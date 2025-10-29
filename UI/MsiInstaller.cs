using System.Diagnostics;
using System.IO;
using System.Management;
using FLaUIDemo.Common;
using NLog;

namespace FLaUIDemo.UI
{
    public static class MsiInstaller
    {
        private static readonly string appName = $"{Configuration.ProcessName} {Configuration.Version}";
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static void Install(string msiPath, params string[] msiParams)
        {
            string exePath = Path.Combine(RegistrySettingsManager.MainDir, $"{Configuration.ProcessName}.exe");
            if (File.Exists(exePath))
            {
                logger.Info($"{appName} is already installed.");
                return;
            }
            
            string paramString = string.Join(" ", msiParams);
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "msiexec.exe",
                Arguments = $"/i " +  msiPath + " " + paramString + " /qn",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process installerProcess = new Process()
            {
                StartInfo = startInfo
            };

            logger.Info($"Installing {appName} ...");
            installerProcess.Start();
            installerProcess.WaitForExit();
            if (installerProcess.ExitCode == 0)
            {
                logger.Info($"'{appName}' installed successfully.");
            }
            else
            {
                logger.Error($"'{appName}' installation failed with exit code: {installerProcess.ExitCode}");
            }
        }
        public static void Unistall()
        {
            logger.Info($"Uninstalling {appName} ...");
            ConnectionOptions options = new ConnectionOptions();
            ManagementScope scope = new ManagementScope("\\\\.\\root\\cimv2", options);
            scope.Connect();
            ObjectQuery query = new ObjectQuery($"SELECT * FROM Win32_Product WHERE Name = '{appName}'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection collection = searcher.Get();
            foreach (ManagementObject obj in collection)
            {
                ManagementBaseObject outParams = obj.InvokeMethod("Uninstall", null, null);
                uint returnValue = (uint)outParams["ReturnValue"];
                if (returnValue == 0)
                {
                    logger.Info($"{appName} uninstalled successfully.");
                }
                else
                {
                    logger.Error($"Failed to uninstall {appName}. Return value: {returnValue}");
                }
            }
        }
    }
}
