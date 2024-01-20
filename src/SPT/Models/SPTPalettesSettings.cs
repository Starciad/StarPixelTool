using MessagePack;

namespace SPT.Models
{
    [MessagePackObject]
    internal struct SPTPalettesSettings
    {
        [Key(0)]
        public string DefinedPalette { get; set; }

        public SPTPalettesSettings()
        {
            this.DefinedPalette = string.Empty;
        }
    }
}
