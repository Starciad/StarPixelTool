using System;
using System.Text;

namespace SPT.Core.IO.Pixelization
{
    /// <summary>
    /// Utility class for checking file compatibility, determining file types, and handling common image and video file extensions.
    /// </summary>
    public static class SPTFilePixelizationCompatibility
    {
        private static readonly string[] imageFileExtensions = [ ".png" ];
        private static readonly string[] videoFileExtensions = [ /* No video formats are currently supported. */ ];

        /// <summary>
        /// Checks if a given file extension is compatible with SPT pixelization.
        /// </summary>
        /// <param name="extension">The file extension to check.</param>
        /// <returns>True if the extension is compatible; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown when the provided extension is null or an empty space.</exception>
        public static bool Check(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new ArgumentException("The provided extension is null or an empty space.", nameof(extension));
            }

            return IsImageExtension(extension) || IsVideoExtension(extension);
        }

        /// <summary>
        /// Gets the <see cref="SPTFilePixelizationType"/> associated with a given file extension.
        /// </summary>
        /// <param name="extension">The file extension to retrieve the <see cref="SPTFilePixelizationType"/> for.</param>
        /// <returns>The <see cref="SPTFilePixelizationType"/> associated with the extension.</returns>
        /// <exception cref="ArgumentException">Thrown when the provided extension is null or an empty space.</exception>
        public static SPTFilePixelizationType GetFileType(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new ArgumentException("The provided extension is null or an empty space.", nameof(extension));
            }

            return IsImageExtension(extension) ? SPTFilePixelizationType.Image : IsVideoExtension(extension) ? SPTFilePixelizationType.Video : SPTFilePixelizationType.Unknown;
        }

        private static bool IsImageExtension(string extension)
        {
            return Array.Exists(imageFileExtensions, x => x.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsVideoExtension(string extension)
        {
            return Array.Exists(videoFileExtensions, x => x.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets a string representing compatible file types for SPT pixelization along with their associated extensions.
        /// </summary>
        /// <returns>A formatted string with file types and extensions.</returns>
        public static string GetCompatibleTypesLabels()
        {
            StringBuilder resultBuilder = new();

            foreach (SPTFilePixelizationType fileType in Enum.GetValues(typeof(SPTFilePixelizationType)))
            {
                if (fileType != SPTFilePixelizationType.Unknown)
                {
                    _ = resultBuilder.AppendLine($"{fileType}: {GetExtensionsLabel(fileType)}");
                }
            }

            return resultBuilder.ToString().TrimEnd();
        }

        private static string GetExtensionsLabel(SPTFilePixelizationType fileType)
        {
            return fileType switch
            {
                SPTFilePixelizationType.Image => string.Join(", ", imageFileExtensions),
                SPTFilePixelizationType.Video => string.Join(", ", videoFileExtensions),
                _ => string.Empty,
            };
        }
    }
}