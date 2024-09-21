using SkiaSharp;

using System.IO;

namespace SPT.GUI.Managers
{
    internal static class SImageManager
    {
        internal static bool IsSourceImageLoaded { get; private set; }
        internal static string SourceImageFileName { get; private set; }
        internal static SKBitmap SourceImageBitmap { get; private set; }

        public static void Load(string fileName, Stream stream)
        {
            IsSourceImageLoaded = true;

            SourceImageFileName = fileName;
            SourceImageBitmap = SKBitmap.Decode(stream);
        }

        public static void Unload()
        {
            if (IsSourceImageLoaded)
            {
                SourceImageFileName = string.Empty;
                SourceImageBitmap.Dispose();
            }

            IsSourceImageLoaded = false;
        }
    }
}
