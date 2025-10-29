using System.Drawing;
using System.Linq.Expressions;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using NLog;

namespace FLaUIDemo.Extensions
{
    public static class ActionExtension
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        static void Click(this AutomationElement element, int x = -1, int y = -1, MouseButton mouseButton = MouseButton.Left)
        {
            if (element != null)
            {
                string elementString = element.ToString();
                int left = element.BoundingRectangle.Left;
                int top = element.BoundingRectangle.Top;
                int width = element.BoundingRectangle.Width;
                int height = element.BoundingRectangle.Height;
                x = x == -1 ? left + (int)width / 2 : left + x;
                y = y == -1 ? top + (int)height / 2 : top + y;

                Point point = new Point(x, y);
                Mouse.MoveTo(point);
                Mouse.Click(mouseButton);
                logger.Info($"Clicked {mouseButton} mouse button on element [ {elementString} ] at ({x},{y})");
            }

        }

        public static void Hover(this AutomationElement element, int x = -1, int y = -1)
        {
            if (element != null)
            {
                string elementString = element.ToString();
                int left = element.BoundingRectangle.Left;
                int top = element.BoundingRectangle.Top;
                int width = element.BoundingRectangle.Width;
                int height = element.BoundingRectangle.Height;
                x = x == -1 ? left + (int)width / 2 : left + x;
                y = y == -1 ? top + (int)height / 2 : top + y;
                
                Point point = new Point(x, y);
                Mouse.MoveTo(point);
                logger.Info($"Mouse hovers element [ {elementString} ]");
            }
        }

        public static void LClick(this AutomationElement element, int x = -1, int y = -1)
        {
            element.Click(x, y, MouseButton.Left);
        }

        public static void RClick(this AutomationElement element, int x = -1, int y = -1)
        {
            element.Click(x, y, MouseButton.Right);
        }
    }
}
