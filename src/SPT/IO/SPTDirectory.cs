using System.IO;

namespace SPT.IO
{
    internal static class SPTDirectory
    {
        internal static string SystemDirectory => Path.Combine(Directory.GetCurrentDirectory(), "System");
        internal static string PalettesDirectory => Path.Combine(Directory.GetCurrentDirectory(), "Palettes");
    }
}
