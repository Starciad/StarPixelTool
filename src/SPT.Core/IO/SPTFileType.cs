namespace SPT.Core.IO
{
    /// <summary>
    /// Enumeration representing the type of file for compatibility checking, categorizing files as Unknown, Image, or Video.
    /// </summary>
    public enum SPTFileType
    {
        /// <summary>
        /// The file type is unknown or not categorized.
        /// </summary>
        Unknown,

        /// <summary>
        /// The file is categorized as an image file.
        /// </summary>
        Image,

        /// <summary>
        /// The file is categorized as a video file.
        /// </summary>
        Video
    }
}