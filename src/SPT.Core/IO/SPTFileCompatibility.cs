using System;
using System.IO;
using System.Text;

namespace SPT.Core.IO
{
    /// <summary>
    /// Utility class for checking file compatibility, determining file types, and handling common image and video file extensions.
    /// </summary>
    public static class SPTFileCompatibility
    {
        private static readonly string[] imageFileExtensions =
        [
            ".png",
            // ".jpg",
            // ".jpeg",
            // ".gif",
            // ".bmp"
        ];

        private static readonly string[] videoFileExtensions =
        [
            // ".mp4",
            // ".avi",
            // ".mov",
            // ".mkv"
        ];

        /// <summary>
        /// Checks if the given file has a compatible extension for image or video.
        /// </summary>
        /// <param name="filename">The full path of the file.</param>
        /// <returns>True if the file has a compatible extension; otherwise, false.</returns>
        public static bool Check(string filename)
        {
            string extension = Path.GetExtension(filename)?.ToLower();

            if (!string.IsNullOrEmpty(extension) && (IsImageExtension(extension) || IsVideoExtension(extension)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the type of file based on its extension, categorizing it as Image, Video, or None.
        /// </summary>
        /// <param name="filename">The full path of the file.</param>
        /// <returns>The type of file as <see cref="SPTFileType"/>.</returns>
        public static SPTFileType GetFileType(string filename)
        {
            string extension = Path.GetExtension(filename)?.ToLower();

            if (IsImageExtension(extension))
            {
                return SPTFileType.Image;
            }
            else if (IsVideoExtension(extension))
            {
                return SPTFileType.Video;
            }

            return SPTFileType.Unknown;
        }

        private static bool IsImageExtension(string extension)
        {
            return Array.Exists(imageFileExtensions, x => x.Equals(extension));
        }

        private static bool IsVideoExtension(string extension)
        {
            return Array.Exists(videoFileExtensions, x => x.Equals(extension));
        }

        /// <summary>
        /// Gets a formatted string with labels for all compatible file types in the project.
        /// </summary>
        /// <returns>A formatted string containing labels for compatible file types.</returns>
        public static string GetCompatibleTypesLabels()
        {
            StringBuilder resultBuilder = new();

            foreach (SPTFileType fileType in Enum.GetValues(typeof(SPTFileType)))
            {
                if (fileType != SPTFileType.Unknown)
                {
                    resultBuilder.AppendLine($"{fileType}: {GetExtensionsLabel(fileType)}");
                }
            }

            return resultBuilder.ToString().TrimEnd();
        }

        private static string GetExtensionsLabel(SPTFileType fileType)
        {
            return fileType switch
            {
                SPTFileType.Image => string.Join(", ", imageFileExtensions),
                SPTFileType.Video => string.Join(", ", videoFileExtensions),
                _ => string.Empty,
            };
        }
    }
}