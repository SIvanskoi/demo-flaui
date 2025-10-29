using FlaUI.Core;
using FlaUI.Core.AutomationElements;


namespace FLaUIDemo.UI
{
    public class DialogOptions : Window
    {
        public DialogOptions(FrameworkAutomationElementBase frameworkAutomationElement) : base(frameworkAutomationElement)
        {
        }

        public Button OK => this.FindFirstDescendant(cf => cf.ByName("OK").And(cf.ByClassName("Button"))).AsButton();
        public Button Cancel => this.FindFirstDescendant(cf => cf.ByAutomationId("2").And(cf.ByClassName("Button"))).AsButton();

    }
}
