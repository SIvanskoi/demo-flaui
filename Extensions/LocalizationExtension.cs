using System;
using System.Collections.Generic;
using System.Data.SQLite;
using FLaUIDemo.Common;


namespace FLaUIDemo.Extensions
{
    public static class LocalizationExtension
    {
        private static readonly Dictionary<string, string> resources = new Dictionary<string, string>()
        {
            //Ribbon resources
            ["New"] = "IDS_BACKSTAGE_OPTION_NEW"
        };
        
        public static string ToL10N(this string str)
        {
            Dictionary<string, string> langs = new Dictionary<string, string>()
            {
                ["ChineseSimplified"] = "CN",
                ["English"] = "EN",
                ["Dutch"] = "NL",
                ["French"] = "FR",
                ["German"] = "DE",
                ["Japanese"] = "JA",
                ["Spanish"] = "ES"
            };
            string localized = String.Empty;
            string resourceLanguage = RegistrySettingsManager.ResourceLanguage;
            string column = langs[resourceLanguage];

            string connectionString = @"Data Source=L10N\l10n.db";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = $"SELECT {column} FROM L10N WHERE SOURCE='{resources[str]}'";
                        command.CommandType = System.Data.CommandType.Text;
                        localized = Convert.ToString(command.ExecuteScalar());
                    }
                    catch(KeyNotFoundException)
                    {
                        localized = str;
                    }
                }
            }
            return localized;
        }
    }
}
