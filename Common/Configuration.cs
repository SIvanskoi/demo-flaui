using System;
using System.IO;
using System.Xml.Linq;

namespace FLaUIDemo.Common
{
    internal static class Configuration
    {
        private static readonly XDocument xDocument = XDocument.Load(Path.Combine(AppContext.BaseDirectory, "testconfig.qa.xml"));
        public static string ProcessName => xDocument.Root.Element("ProcessName").Value;
        public static string AccountEmail => xDocument.Root.Element("AccountEmail").Value;
        public static string AccountPassword => xDocument.Root.Element("AccountPassword").Value;
        public static string CloudUrl => xDocument.Root.Element("CloudURL").Value;
        public static int CurveSmoothingDisabled => Convert.ToInt32(xDocument.Root.Element("CurveSmoothingDisabled").Value);
        public static int InterpolationSmoothingDisabled => Convert.ToInt32(xDocument.Root.Element("InterpolationSmoothingDisabled").Value);
        public static int TextSmoothingDisabled => Convert.ToInt32(xDocument.Root.Element("TextSmoothingDisabled").Value);
        public static string Version => xDocument.Root.Element("Version").Value;
        public static string WebView2DebuggingPort => xDocument.Root.Element("WebView2DebuggingPort").Value;

    }
}
