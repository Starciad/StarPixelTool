using MessagePack;

using SPT.Constants;
using SPT.IO;
using SPT.Models;

using System;
using System.IO;

namespace SPT.Managers
{
    internal static class SPTSettingsManager
    {
        internal static void Initialize()
        {
            EnsureSettingsFileExists(SPTFileConstants.FileSettings, () => new SPTFileSettings());
            EnsureSettingsFileExists(SPTFileConstants.PaletteSettings, () => new SPTPalettesSettings());
        }

        internal static void CreateFileSettings(SPTFileSettings value)
        {
            CreateSettingsFile(value, SPTFileConstants.FileSettings);
        }
        internal static void CreatePalettesSettings(SPTPalettesSettings value)
        {
            CreateSettingsFile(value, SPTFileConstants.PaletteSettings);
        }

        internal static SPTFileSettings GetFileSettings()
        {
            return GetSettingsFromFile<SPTFileSettings>(SPTFileConstants.FileSettings);
        }
        internal static SPTPalettesSettings GetPalettesSettings()
        {
            return GetSettingsFromFile<SPTPalettesSettings>(SPTFileConstants.PaletteSettings);
        }

        private static void EnsureSettingsFileExists<T>(string fileName, Func<T> createDefaultSettings)
        {
            string directoryPath = SPTDirectory.SystemDirectory;
            string filePath = Path.Combine(directoryPath, fileName);

            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(directoryPath);
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