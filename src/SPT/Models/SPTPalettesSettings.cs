﻿using MessagePack;

namespace SPT.Models
{
    [MessagePackObject]
    public struct SPTPalettesSettings
    {
        [Key(0)]
        public string DefinedPalette { get; set; }

        public SPTPalettesSettings()
        {
            this.DefinedPalette = string.Empty;
        }
    }
}
