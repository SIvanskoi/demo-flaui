using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using FLaUIDemo.Browser;
using FLaUIDemo.COM;
using FLaUIDemo.Extensions;
using FLaUIDemo.UI;
using NLog;

namespace FLaUIDemo.Common
{
    public sealed class Robot : IDisposable
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private Application application = null;
        private dynamic com = null;
        private readonly AutomationBase automation = new UIA3Automation();
        
        public MainWindow MainWindow => application.GetMainWindow(automation, TimeSpan.FromSeconds(30)).As<MainWindow>();

        public ComInterop ComInstance => com;  

        public void AttachOrLaunch()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = Path.Combine(RegistrySettingsManager.MainDir, $"{Configuration.ProcessName}.exe")
            };
            application = Application.AttachOrLaunch(psi);
            application.WaitWhileMainHandleIsMissing(TimeSpan.FromSeconds(30));
            MainWindow.SetForeground();
            com = ComInterop.Application;
        }

        public dynamic AddBlankMap()
        {
            int count = com.AllDocuments.Count; //ComInterop.Application.AllDocuments.Count;
            var button = MainWindow.RibbonBar.New;
            if (button != null)
            {
                button.LClick();
            }
            else
            {
                Keyboard.Press(VirtualKeyShort.ESCAPE);
                MainWindow.RibbonBar.New.LClick();
            }
            count++;
            bool result = Retry.WhileFalse(() => com.AllDocuments.Count == count, TimeSpan.FromSeconds(5)).Success;
            if (result)
                return com.AllDocuments[count];
            return null;
        }

        public dynamic AddExistingMap(string filePath)
        {
            int count = com.AllDocuments.Count;
            var button = MainWindow.RibbonBar.Open;
            if (button != null)
            {
                button.LClick();
            }
            else
            {
                Keyboard.Press(VirtualKeyShort.ESCAPE);
                MainWindow.RibbonBar.Open.LClick();
            }
            var fileDialog = MainWindow.OpenFileDialog;
            fileDialog.FileName.Text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            fileDialog.Open.LClick();
            count++;
            bool result = Retry.WhileFalse(() => com.AllDocuments.Count == count, TimeSpan.FromSeconds(5)).Success;
            if (result)
                return com.AllDocuments[count];
            return null;
        }

        public dynamic AddFloatingTopic(int x, int y)
        {
            MainWindow.SetForeground();
            SetSelection();
            var collection = com.ActiveDocument.AllFloatingTopics;
            int count = (int)collection.Count;
            MainWindow.RibbonBar.Click("Insert")
                                .Click("Map Topic", ControlType.SplitButton, false);
            ClickMenuItem("Add Floating Map Topic");
            MainWindow.ActiveDocumentWindow.LClick(x, y);
            bool res = Retry.WhileTrue(() => collection.Count == count,
                                        TimeSpan.FromMilliseconds(500)).Success;
            if (res)
            {
                var topic = collection[collection.Count];
                Rectangle rect = ComInterop.GetObjectRect(topic);
                Point center = rect.Center();
                Mouse.MoveTo(center);
                return topic;
            }
            return null;
        }

        public Robot ClickMenuItem(string itemName)
        {
            MainWindow.WaitUntilExists(itemName, ControlType.MenuItem).AsMenuItem().LClick();
            return this;
        }

        public void Drag(Point startingPoint, Point endingPoint, VirtualKeyShort? skShift = null, MouseButton mouseButton = MouseButton.Left)
        {
            double speed = Mouse.MovePixelsPerMillisecond;
            Mouse.MovePixelsPerMillisecond = 0.5;
            int dx = endingPoint.X - startingPoint.X;
            int dy = endingPoint.Y - startingPoint.Y;
            int totalDistance = (int)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            int steps = Math.Max(Convert.ToInt32(totalDistance / Mouse.MovePixelsPerStep), 1);
            Point[] points = new Point[steps];
            points[0] = startingPoint;
            points[steps - 1] = endingPoint;
            dx = dx == 0 ? 1 : dx;
            double slope = (double)dy / dx;
            double x, y;
            for (double i = 1; i < steps - 1; i++)
            {
                y = slope == 0 ? 0 : dy * (i / steps);
                x = slope == 0 ? dx * (i / steps) : y / slope;
                points[(int)i] = new Point((int)x + startingPoint.X, (int)y + startingPoint.Y);
            }

            var duration = TimeSpan.FromMilliseconds(Convert.ToInt32(totalDistance / Mouse.MovePixelsPerMillisecond));
            var interval = TimeSpan.FromMilliseconds(duration.TotalMilliseconds / steps);

            Mouse.Position = points[0];
            Mouse.Down(mouseButton);
            if (skShift != null)
            {
                Keyboard.Press((VirtualKeyShort)skShift);
            }
            Wait.UntilInputIsProcessed();
            for (int i = 1; i < points.Length; i++)
            {
                Mouse.Position = points[i];
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(interval.TotalMilliseconds));
            }
            Mouse.Up(mouseButton);
            if (skShift != null)
            {
                Keyboard.Release((VirtualKeyShort)skShift);
            }
            Wait.UntilInputIsProcessed();
            Mouse.MovePixelsPerMillisecond = speed;
            logger.Info($"Mouse dragged from {startingPoint.ToString()} to {endingPoint.ToString()}");
        }
        
        public void Kill()
        {             
            if (application != null && !application.HasExited)
            {
                application.Kill();
                application.Dispose();
                application = null;
            }
        }

        public async Task LogIn()
        {
            if (RegistrySettingsManager.IsAccountMode) 
                return;
            var loginWindow = Retry.WhileNull(() => MainWindow.FindFirstDescendant(cf => cf.ByClassName("EdgeBrowserHost")), TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(500)).Result;
            var browser = new WebBrowser();
            await browser.ConnectToWebView2();
            await browser.AccountPage.LogIn();
            System.Threading.Thread.Sleep(5000);
            Retry.WhileFalse(() => RegistrySettingsManager.InstallUserDataInProgress == 0, TimeSpan.FromSeconds(180), TimeSpan.FromSeconds(1));
            System.Threading.Thread.Sleep(5000);
            Retry.WhileFalse(() => RegistrySettingsManager.InstallUserDataInProgress == 0, TimeSpan.FromSeconds(180), TimeSpan.FromSeconds(1));
            MainWindow.WaitUntilClickable(TimeSpan.FromSeconds(5));
        }

        public void SaveAs(string filename)
        {
            var button = MainWindow.RibbonBar.Save;
            if (button != null)
            {
                button.LClick();
            }
            else
            {
                Keyboard.Press(VirtualKeyShort.ESCAPE);
                MainWindow.RibbonBar.Save.LClick();
            }
            MainWindow.ChooseLocationDialog.Local.LClick();
            var saveFileDialog = MainWindow.SaveFileDialog;
            saveFileDialog.FileName.Text = filename;
            saveFileDialog.Save.LClick();
        }

        public void SetSelection(params dynamic[] elements)
        {
            com.ActiveDocument.Selection.RemoveAll();
            if (elements != null)
            {
                foreach (var elem in elements)
                {
                    com.ActiveDocument.Selection.Add(elem);
                }
            }
        }

        public DialogOptions ShowOptions()
        {
            MainWindow.PostMessage(WindowMessages.WM_COMMAND, (IntPtr)WindowMessages.ID_TOOLS_OPTIONS, IntPtr.Zero);
            var window = MainWindow.WaitUntilExists($"{Configuration.ProcessName} Options", ControlType.Window);
            //MainWindow.ModalWindows.FirstOrDefault(wnd => wnd.Title.Equals($"{Configuration.ProcessName} Options"));
            //var optionsWindow = Retry.WhileNull(() => MainWindow.FindFirstDescendant(cf => cf.ByName("{Configuration.ProcessName} Options")), TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(500)).Result;
            return window.As<DialogOptions>();
        }

        public void Dispose()
        {
            ComInterop.SafeRelease(com);
            application.Close();
            application.Dispose();
        }

    }
}
