using System;
using System.IO;
using Newtonsoft.Json;
namespace ServerPatches
{
    class SettingsInstance
    {
        public bool LoggingAddGameLogsMessages = false;
        public int NumberOfLogsToKeep = 10;
        public int NumberOfRestLogsToKeep = 3;
        public string SettingsVersion = "1.0.0";
    }

    public class Settings
    {

        private static SettingsInstance _instance;
        

        public static void LoadSettings(string jsonPath)
        {
            Directory.CreateDirectory("ServerPatches");

            if(File.Exists(jsonPath))
            {
                SettingsInstance readIn = JsonConvert.DeserializeObject<SettingsInstance>(File.ReadAllText(jsonPath));
                if(readIn != null )
                {
                    _instance = readIn;
                }
            }
            else
            {
                _instance = new SettingsInstance();

                // Settings doesn't exist, save
                SaveSettings(jsonPath);
            }
        }

        public static void SaveSettings(string jsonPath)
        {
            File.WriteAllText(jsonPath,JsonConvert.SerializeObject(_instance,Formatting.Indented));
        }

        public static int GetNumberOfLogsToKeep()
        {
            return _instance.NumberOfLogsToKeep;
        }
        public static int GetNumberOfRestLogsToKeep()
        {
            return _instance.NumberOfRestLogsToKeep;
        }

        public static bool AreStoringAddGameLogsMessagesToLogFile()
        {
            return _instance.LoggingAddGameLogsMessages;
        }

    }
}
