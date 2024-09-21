using MessagePack;

using SPT.IO.Constants;
using SPT.IO.Models;

using System;
using System.IO;

namespace SPT.IO
{
    public static class SPTSettingSystemFiles
    {
        public static void Initialize()
        {
            EnsureSettingsFileExists(SPTFileConstants.FileSettings, () => new SPTFileSettings() { InputFilename = "a", OutputFilename = "b" });
            EnsureSettingsFileExists(SPTFileConstants.PaletteSettings, () => new SPTPalettesSettings());
        }

        public static void CreateFileSettings(SPTFileSettings value)
        {
            CreateSettingsFile(value, SPTFileConstants.FileSettings);
        }
        public static void CreatePalettesSettings(SPTPalettesSettings value)
        {
            CreateSettingsFile(value, SPTFileConstants.PaletteSettings);
        }

        public static SPTFileSettings GetFileSettings()
        {
            return GetSettingsFromFile<SPTFileSettings>(SPTFileConstants.FileSettings);
        }
        public static SPTPalettesSettings GetPalettesSettings()
        {
            return GetSettingsFromFile<SPTPalettesSettings>(SPTFileConstants.PaletteSettings);
        }

        private static void EnsureSettingsFileExists<T>(string fileName, Func<T> createDefaultSettings)
        {
            string directoryPath = SPTDirectory.SystemDirectory;
            string filePath = Path.Combine(directoryPath, fileName);

            if (!File.Exists(filePath))
            {
                _ = Directory.CreateDirectory(directoryPath);
                CreateSettingsFile(createDefaultSettings(), fileName);
            }
        }

        private static void CreateSettingsFile<T>(T value, string fileName)
        {
            string filePath = Path.Combine(SPTDirectory.SystemDirectory, fileName);
            File.WriteAllBytes(filePath, MessagePackSerializer.Serialize(value));
        }
        private static T GetSettingsFromFile<T>(string fileName)
        {
            string path = Path.Combine(SPTDirectory.SystemDirectory, fileName);

            if (File.Exists(path))
            {
                using FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read);
                return MessagePackSerializer.Deserialize<T>(fs);
            }
            else
            {
                return default(T);
            }
        }
    }
}