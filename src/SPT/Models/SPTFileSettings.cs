using MessagePack;

namespace SPT.Models
{
    [MessagePackObject]
    internal struct SPTFileSettings
    {
        [Key(0)]
        internal string InputFilename { get; set; }

        [Key(1)]
        internal string OutputFilename { get; set; }

        public SPTFileSettings()
        {
            this.InputFilename = string.Empty;
            this.OutputFilename = string.Empty;
        }
    }
}
