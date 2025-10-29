using System;
using FlaUI.Core;
using FlaUI.Core.Definitions;
using FlaUI.Core.AutomationElements;
using FLaUIDemo.Extensions;


namespace FLaUIDemo.UI
{
    public class RibbonBar : Window
    {
        public RibbonBar(FrameworkAutomationElementBase frameworkAutomationElement) : base(frameworkAutomationElement)
        {
        }

        public Button New => this.WaitUntilExists("New", ControlType.SplitButton, TimeSpan.FromSeconds(0), false).AsButton();
        public Button Open => this.WaitUntilExists("Open", ControlType.Button, TimeSpan.FromSeconds(0), false).AsButton();
        public Button Save => this.WaitUntilExists("Save", ControlType.Button, TimeSpan.FromSeconds(0), false).AsButton();

        public RibbonBar Click(string name, ControlType? controlType = null, bool primaryAction = true )
        {
            if (controlType == null)
            {
                var toolBar = this.WaitUntilExists(name, ControlType.ToolBar, TimeSpan.FromSeconds(0), false);
                if (toolBar == null)
                {
                    var item = this.WaitUntilExists(name, ControlType.TabItem);
                    item.Click();
                }
            }
            else
            {
                var item = this.WaitUntilExists(name, (ControlType)controlType);
                if (primaryAction)
                    item.Click();
                else
                {
                    int height = item.BoundingRectangle.Height;
                    item.LClick(y: (int)(height * 0.8));
                }
            }
            return this;
        }

        /*
        public RibbonBar this[string name, ControlType? controlType = null]
        {
            get
            {
                if (controlType == null)
                {
                    var toolBar = FindFirstDescendant(cf => cf.ByControlType(ControlType.ToolBar).And(cf.ByName(name)));
                    if (toolBar == null)
                    {
                        var item = this.FindFirstDescendant(cf => cf.ByControlType(ControlType.TabItem).And(cf.ByName(name)));
                        item.Click();
                    }
                }
                else
                {
                    var item = this.FindFirstDescendant(cf => cf.ByControlType((ControlType)controlType).And(cf.ByName(name)));
                    item.Click();
                }
                return this;
            }
        }
        */
            
    }
}
