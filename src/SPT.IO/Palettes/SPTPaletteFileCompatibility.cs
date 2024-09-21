using System;
using System.Linq;

namespace SPT.IO.Palettes
{
    /// <summary>
    /// Provides functionality for checking and retrieving compatibility information for <see cref="SPTPalette"/> types.
    /// </summary>
    public static class SPTPaletteFileCompatibility
    {
        /// <summary>
        /// Get all literal extensions (that include '.' in their name) currently supported.
        /// </summary>
        public static string[] Extensions => fileInfos.Select(x => x.extension).ToArray();

        /// <summary>
        /// Supported palette fileInfos along with their corresponding <see cref="SPTPalette"/>.
        /// </summary>
        private static readonly (string extension, SPTPaletteFileType paletteType)[] fileInfos =
        [
            (".gpl", SPTPaletteFileType.GPL),
        ];

        /// <summary>
        /// Checks if a given extension is compatible with any known <see cref="SPTPalette"/> type.
        /// </summary>
        /// <param name="extension">The file extension to check.</param>
        /// <returns>True if the extension is compatible; otherwise, false.</returns>
        public static bool Check(string extension)
        {
            return string.IsNullOrWhiteSpace(extension)
                ? throw new ArgumentException("The provided extension is null or an empty space.", nameof(extension))
                : Array.Exists(fileInfos, x => x.extension.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the <see cref="SPTPalette"/> associated with a given extension.
        /// </summary>
        /// <param name="extension">The file extension to retrieve the <see cref="SPTPalette"/> for.</param>
        /// <returns>The <see cref="SPTPalette"/> associated with the extension.</returns>
        /// <exception cref="ArgumentException">Thrown when the provided extension is null or an empty space.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the extension is not associated with any SPTPaletteType.</exception>
        public static SPTPaletteFileType GetPaletteType(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new ArgumentException("The provided extension is null or an empty space.", nameof(extension));
            }

            (string extension, SPTPaletteFileType paletteType) paletteTypeInfo = Array.Find(fileInfos, x => x.extension.Equals(extension, StringComparison.OrdinalIgnoreCase));

            return paletteTypeInfo.Equals(default((string Extension, SPTPaletteFileType Type)))
                ? throw new InvalidOperationException($"No SPTPaletteType associated with the extension '{extension}'.")
                : paletteTypeInfo.paletteType;
        }

        /// <summary>
        /// Gets the file extension associated with a given <see cref="SPTPalette"/>.
        /// </summary>
        /// <param name="paletteType">The <see cref="SPTPalette"/> to retrieve the file extension for.</param>
        /// <returns>The file extension associated with the <see cref="SPTPalette"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the paletteType is not associated with any file extension.</exception>
        public static string GetPaletteExtension(SPTPaletteFileType paletteType)
        {
            (string extension, SPTPaletteFileType paletteType) extensionInfo = Array.Find(fileInfos, x => x.paletteType.Equals(paletteType));

            return extensionInfo.Equals(default((string Extension, SPTPaletteFileType Type)))
                ? throw new InvalidOperationException($"No file extension associated with the SPTPaletteType '{paletteType}'.")
                : extensionInfo.extension;
        }

        /// <summary>
        /// Gets a string representing all compatible file types for <see cref="SPTPalette"/>.
        /// </summary>
        /// <returns>A comma-separated string of compatible file types.</returns>
        public static string GetCompatibleTypesLabels()
        {
            return string.Join(", ", fileInfos.Select(x => x.extension));
        }
    }
}