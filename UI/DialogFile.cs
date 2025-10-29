using FlaUI.Core;
using FlaUI.Core.AutomationElements;


namespace FLaUIDemo.UI
{
    public class OpenFileDialog : Window
    {
        public OpenFileDialog(FrameworkAutomationElementBase frameworkAutomationElement) : base(frameworkAutomationElement)
        {
        }

        public TextBox FileName => FindFirstDescendant(cf => cf.ByAutomationId("1148").And(cf.ByClassName("Edit"))).AsTextBox();
        public Button Open => FindFirstDescendant(cf => cf.ByAutomationId("1").And(cf.ByClassName("Button"))).AsButton();
        public Button Cancel => FindFirstDescendant(cf => cf.ByAutomationId("2").And(cf.ByClassName("Button"))).AsButton();

    }

    public class SaveFileDialog : Window
    {
        public SaveFileDialog(FrameworkAutomationElementBase frameworkAutomationElement) : base(frameworkAutomationElement)
        {
        }

        public TextBox FileName => FindFirstDescendant(cf => cf.ByAutomationId("1001").And(cf.ByClassName("Edit"))).AsTextBox();

        public Button Save => FindFirstDescendant(cf => cf.ByAutomationId("1").And(cf.ByClassName("Button"))).AsButton();
    }
}
