using MessagePack;

namespace SPT.Models
{
    [MessagePackObject]
    public struct SPTFileSettings
    {
        [Key(0)]
        public string InputFilename { get; set; }

        [Key(1)]
        public string OutputFilename { get; set; }

        public SPTFileSettings()
        {
            this.InputFilename = string.Empty;
            this.OutputFilename = string.Empty;
        }
    }
}
