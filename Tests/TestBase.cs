using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FlaUI.Core.Input;
using FLaUIDemo.Common;
using FLaUIDemo.Extensions;
using FLaUIDemo.UI;
using NUnit.Framework; 

namespace FLaUIDemo.Tests
{
    public class TestBase
    {
        private readonly Robot robot = new Robot();

        public Robot Robot => robot;

        [OneTimeSetUp]
        public async Task InstallAndLogin()
        {
            Mouse.MovePixelsPerMillisecond = 2;
            MsiInstaller.Install(Path.Combine(@"C:\MATC2", $"{Configuration.ProcessName}SetupAdmin.msi"), "INTERNETACCESS=\"0\"", "JIRACLOUDACCESSOVERRIDE=\"1\"");

            RegistrySettingsManager.BCGAutomationEnabled = 1;
            RegistrySettingsManager.DisableBackstageOnStart = 1;
            RegistrySettingsManager.DisplaySplash = 0;
            RegistrySettingsManager.IsFirstTimeForUser = 0;
            RegistrySettingsManager.ShowFloatingToolbarWithLClick = 0;
            RegistrySettingsManager.SnapToGridDuringDragAndDrop = 0;
            
            // Pointing product to test environment
            RegistrySettingsManager.MindjetCloudUrl = Configuration.CloudUrl;
            RegistrySettingsManager.MindjetCloudWebUrl = Configuration.CloudUrl;

            // Disabling smoothing for tests
            RegistrySettingsManager.CurveSmoothingDisabled = Configuration.InterpolationSmoothingDisabled;
            RegistrySettingsManager.InterpolationSmoothingDisabled = Configuration.InterpolationSmoothingDisabled;
            RegistrySettingsManager.TextSmoothingDisabled = Configuration.TextSmoothingDisabled;

            // Define webview2 debugging port
            string browserArgumentsKey = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Edge\WebView2\AdditionalBrowserArguments";
            RegistrySettingsManager.SetValue(browserArgumentsKey, $"{Configuration.ProcessName}.exe", $"--remote-debugging-port={Configuration.WebView2DebuggingPort}");
            RegistrySettingsManager.SetValue(browserArgumentsKey, $"{Configuration.ProcessName}Snap.exe", $"--remote-debugging-port={Configuration.WebView2DebuggingPort}");
            //string htmOpenKey = @"HKEY_LOCAL_MACHINE\Software\Classes\MSEdgeHTM\shell\open\command";
            //RegistrySettingsManager.SetValue(htmOpenKey, "", $"\"{Path.Combine(Registry

            
            Robot.AttachOrLaunch();
            await Robot.LogIn();
            var optionsDialog = Robot.ShowOptions();
            optionsDialog.OK.LClick();
            
        }

        [TearDown]
        public void TearDown()
        {
            //Robot.Dispose();
            foreach (Process process in Process.GetProcessesByName("msedge"))
            {
                TestContext.WriteLine($"Killing process {process.Id} - {process.ProcessName}");
                process.Kill();
            }
        }
    }
}
