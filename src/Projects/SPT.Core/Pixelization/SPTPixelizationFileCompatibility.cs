﻿using System;
using System.Text;

namespace SPT.Core.Pixelization
{
    /// <summary>
    /// Utility class for checking file compatibility, determining file types, and handling common image and video file extensions.
    /// </summary>
    public static class SPTPixelizationFileCompatibility
    {
        private static readonly string[] imageFileExtensions = [".png"];
        private static readonly string[] videoFileExtensions = [ /* No video formats are currently supported. */ ];

        /// <summary>
        /// Checks if a given file extension is compatible with SPT pixelization.
        /// </summary>
        /// <param name="extension">The file extension to check.</param>
        /// <returns>True if the extension is compatible; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown when the provided extension is null or an empty space.</exception>
        public static bool Check(string extension)
        {
            return string.IsNullOrWhiteSpace(extension)
                ? throw new ArgumentException("The provided extension is null or an empty space.", nameof(extension))
                : IsImageExtension(extension) || IsVideoExtension(extension);
        }

        /// <summary>
        /// Gets the <see cref="SPTPixelizationFileType"/> associated with a given file extension.
        /// </summary>
        /// <param name="extension">The file extension to retrieve the <see cref="SPTPixelizationFileType"/> for.</param>
        /// <returns>The <see cref="SPTPixelizationFileType"/> associated with the extension.</returns>
        /// <exception cref="ArgumentException">Thrown when the provided extension is null or an empty space.</exception>
        public static SPTPixelizationFileType GetFileType(string extension)
        {
            return string.IsNullOrWhiteSpace(extension)
                ? throw new ArgumentException("The provided extension is null or an empty space.", nameof(extension))
                : IsImageExtension(extension) ? SPTPixelizationFileType.Image : IsVideoExtension(extension) ? SPTPixelizationFileType.Video : SPTPixelizationFileType.Unknown;
        }

        /// <summary>
        /// Gets a string representing compatible file types for SPT pixelization along with their associated extensions.
        /// </summary>
        /// <returns>A formatted string with file types and extensions.</returns>
        public static string GetCompatibleTypesLabels()
        {
            StringBuilder resultBuilder = new();

            foreach (SPTPixelizationFileType fileType in Enum.GetValues(typeof(SPTPixelizationFileType)))
            {
                if (fileType != SPTPixelizationFileType.Unknown)
                {
                    _ = resultBuilder.AppendLine($"{fileType}: {GetExtensionsLabel(fileType)}");
                }
            }

            return resultBuilder.ToString().TrimEnd();
        }

        private static bool IsImageExtension(string extension)
        {
            return Array.Exists(imageFileExtensions, x => x.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsVideoExtension(string extension)
        {
            return Array.Exists(videoFileExtensions, x => x.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        private static string GetExtensionsLabel(SPTPixelizationFileType fileType)
        {
            return fileType switch
            {
                SPTPixelizationFileType.Image => imageFileExtensions.Length == 0 ? "No image formats are supported." : string.Join(", ", imageFileExtensions),
                SPTPixelizationFileType.Video => videoFileExtensions.Length == 0 ? "No video formats are supported." : string.Join(", ", videoFileExtensions),
                _ => string.Empty,
            };
        }
    }
}