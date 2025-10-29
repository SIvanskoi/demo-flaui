using System;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FLaUIDemo.COM;
using FLaUIDemo.Extensions;
using NLog;


namespace FLaUIDemo.UI
{
    public class MainWindow : Window
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public MainWindow(FrameworkAutomationElementBase frameworkAutomationElement) : base(frameworkAutomationElement)
        {
        }

        public Window ActiveDocumentWindow
        {
            get
            {
                try
                {
                    var app = ComInterop.Application;
                    if (app != null && app.ActiveDocument != null)
                    {
                        string caption = app.ActiveDocument.Name as string;
                        var workspaceWnd = this.WaitUntilExists("Workspace".ToL10N(), ControlType.Pane);
                        var collection = workspaceWnd.FindAllDescendants(cf => cf.ByControlType(ControlType.Window));
                        foreach (var wnd in collection)
                        {
                            if (wnd.Name != null && wnd.Name.Contains(caption))
                            {
                                var docWnd = wnd.FindFirstDescendant(cf => cf.ByClassName("AfxFrameOrView140u"));
                                return docWnd.AsWindow();
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    logger.Fatal(ex, "Failed to get Active document window");
                    throw;
                }
                return null;
            }
        }

        public RibbonBar RibbonBar => FindFirstDescendant(cf => cf.ByAutomationId("59398")).As<RibbonBar>();
        public ChooseLocationDialog ChooseLocationDialog => this.WaitUntilExists("Choose Location".ToL10N(), ControlType.Window).As<ChooseLocationDialog>(); //FindFirstDescendant(cf => cf.ByName("Choose Location".GetLocalizedString()).And(cf.ByClassName("#32770"))).As<ChooseLocationDialog>();
        public OpenFileDialog OpenFileDialog => this.WaitUntilExists("Open File".ToL10N(), ControlType.Window).As<OpenFileDialog>();
        public SaveFileDialog SaveFileDialog => this.WaitUntilExists("Save As".ToL10N(), ControlType.Window).As<SaveFileDialog>();
    }
}
