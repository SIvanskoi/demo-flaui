using System.IO;
using Microsoft.Win32;
using FLaUIDemo.Common;
using NLog;


namespace FLaUIDemo.Common
{
    internal static class RegistrySettingsManager
    {
        private static readonly string registryKeyPath = $"Software\\Mindjet\\{Configuration.ProcessName}";
        private static readonly string hklmBasePath = Path.Combine("HKEY_LOCAL_MACHINE", registryKeyPath, Configuration.Version);
        private static readonly string hkcuBasePath = Path.Combine("HKEY_CURRENT_USER", registryKeyPath, Configuration.Version);
        private static readonly string keyInstaller = "Installer";
        private static readonly string keyRenderer = "Renderer";
        private static readonly string keySettings = "Settings";

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static void DeleteSettingsEntry()
        {
            string _path = Path.Combine(registryKeyPath);
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_path, true))
            {
                key?.DeleteSubKeyTree(Configuration.Version, false);
            }            
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(_path, true))
            {
                key?.DeleteSubKeyTree(Configuration.Version, false);
            }
        }

        public static object GetValue(string registryKeyPath, string value)
        {
            object retVal = Registry.GetValue(registryKeyPath, value, null);
            return retVal;
        }

        public static void SetValue<T>(string registryKeyPath, string value, T data)
        {
            Registry.SetValue(registryKeyPath, value, data);
        }

        public static int BCGAutomationEnabled
        {
            set
            {
                string _path = Path.Combine(hklmBasePath, keySettings);
                SetValue<int>(_path, "BCGAutomationEnabled", value);
            }
        }

        public static int CurveSmoothingDisabled
        {
            set
            {
                string _path = Path.Combine(hkcuBasePath, keyRenderer);
                SetValue(_path, "CurveSmoothingDisabled", value);
            }
        }

        public static int? DisableBackstageOnStart
        {
            get
            {
                string _path = Path.Combine(hkcuBasePath, keySettings);
                return GetValue(_path, "DisableBackstageOnStart") as int?;
            }
            set
            {
                string _path = Path.Combine(hkcuBasePath, keySettings);
                SetValue(_path, "DisableBackstageOnStart", (int)value);
            }
        }

        public static int? DisplaySplash
        {
            get
            {
                string _path = Path.Combine(hkcuBasePath, keySettings);
                return GetValue(_path, "DisplaySplash") as int?;
            }
            set
            {
                string _path = Path.Combine(hkcuBasePath, keySettings);
                SetValue(_path, "DisplaySplash", (int)value);
            }
        }

        public static int? InstallUserDataInProgress
        {
            get
            {
                string _path = Path.Combine(hkcuBasePath, keySettings);
                return GetValue(_path, "InstallUserDataInProgress") as int?;
            }
            set
            {
                string _path = Path.Combine(hkcuBasePath, keySettings);
                SetValue(_path, "InstallUserDataInProgress", (int)value);
            }
        }

        public static int InterpolationSmoothingDisabled
        {
            set
            {
                string _path = Path.Combine(hkcuBasePath, keyRenderer);
                SetValue(_path, "InterpolationSmoothingDisabled", value);
            }
        }

        public static bool IsAccountMode
        {
            get
            {
                string _path = Path.Combine(hkcuBasePath, keySettings);
                string ret = GetValue(_path, Configuration.CloudUrl) as string;
                return !string.IsNullOrEmpty(ret);
            }
        }

        public static int? IsFirstTimeForUser
        {
            get
            {
                string _path = Path.Combine(hkcuBasePath, keySettings);
                return GetValue(_path, "IsFirstTimeForUser") as int?;
            }
            set
            {
                string _path = Path.Combine(hkcuBasePath, keySettings);
                SetValue(_path, "IsFirstTimeForUser", (int)value);
            }
        }

        public static string LocalUserDataDirectory
        {             
            get
            {
                string _path = Path.Combine(hkcuBasePath, keySettings);
                return (string)GetValue(_path, "LocalUserDataDirectory") ?? string.Empty;
            }
        }

        public static string MainDir
        {
            get
            {
                string _path = Path.Combine(hklmBasePath, keyInstaller);
                return (string)GetValue(_path, "MainDir") ?? string.Empty;
            }
        }

        public static string MindjetCloudUrl
        {
            set
            {
                string _path = Path.Combine(hklmBasePath, keySettings);
                SetValue(_path, "MindjetCloudUrl", value);
            }
        }

        public static string MindjetCloudWebUrl
        {
            set
            {
                string _path = Path.Combine(hklmBasePath, keySettings);
                SetValue(_path, "MindjetCloudWebUrl", value);
            }
        }

        public static string ResourceLanguage
        {
            get
            {
                string valueName = "ResourceLanguage";
                string _path = Path.Combine(hkcuBasePath, keySettings);
                string retVal = (string)GetValue(_path, valueName) ?? string.Empty;
                if (string.IsNullOrEmpty(retVal))
                {
                    _path = Path.Combine(hklmBasePath, keySettings);
                    retVal = (string)GetValue(_path, valueName) ?? string.Empty;
                }
                return retVal;
            }
        }

        public static int ShowFloatingToolbarWithLClick
        {
            set
            {
                string _path = Path.Combine(hkcuBasePath, keySettings);
                SetValue(_path, "ShowFloatingToolbarWithLClick", value);
            }
        }

        public static int SnapToGridDuringDragAndDrop
        {
            set
            {
                string _path = Path.Combine(hkcuBasePath, keySettings);
                SetValue(_path, "SnapToGridDuringDragAndDrop2", value);
            }
        }

        public static int TextSmoothingDisabled
        {
            set
            {
                string _path = Path.Combine(hkcuBasePath, keyRenderer);
                SetValue(_path, "TextSmoothingDisabled", value);
            }
        }

    }
}
