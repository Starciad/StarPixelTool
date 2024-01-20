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
    }
}
