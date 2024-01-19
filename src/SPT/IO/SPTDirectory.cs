using System.IO;

namespace SPT.IO
{
    internal static class SPTDirectory
    {
        internal static string PalettesDirectory => Path.Combine(Directory.GetCurrentDirectory(), "Palettes");
    }
}
