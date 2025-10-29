using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FLaUIDemo.Extensions;

namespace FLaUIDemo.UI
{
    public class ChooseLocationDialog : Window
    {
        public ChooseLocationDialog(FrameworkAutomationElementBase frameworkAutomationElement) : base(frameworkAutomationElement)
        {
        }

        public Button Local => this.WaitUntilExists("Local".ToL10N(), ControlType.Button).AsButton();
    }
}
