using MessagePack;

using System.IO;

namespace SPT.Models
{
    [MessagePackObject]
    internal struct SPTPalettesSettings
    {
        [Key(0)]
        public string DefinedPalette { get; set; }
    }
}
