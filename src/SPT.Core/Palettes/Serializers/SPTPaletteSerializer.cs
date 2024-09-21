using SPT.IO.Palettes;

using System;
using System.IO;

namespace SPT.Core.Palettes.Serializers
{
    /// <summary>
    /// Provides methods for serializing and deserializing <see cref="SPTPalette"/> objects.
    /// </summary>
    public static class SPTPaletteSerializer
    {
        /// <summary>
        /// Deserializes an <see cref="SPTPalette"/> object from a color palette file.
        /// </summary>
        /// <param name="filename">The path to the color palette file.</param>
        /// <returns>The deserialized <see cref="SPTPalette"/> object.</returns>
        /// <exception cref="ArgumentException">Thrown when the provided filename is null or an empty space.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the specified file filename does not exist.</exception>
        /// <exception cref="NotSupportedException">Thrown when the specified color palette file is not supported by the system.</exception>
        public static SPTPalette Deserialize(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("The provided filename is null or empty.", nameof(filename));
            }

            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("The specified file filename does not exist.", filename);
            }

            string extension = Path.GetExtension(filename);

            return !SPTPaletteFileCompatibility.Check(extension)
                ? throw new NotSupportedException("The specified color palette file is not supported by the system.")
                : SPTPaletteFileCompatibility.GetPaletteType(extension) switch
                {
                    SPTPaletteFileType.Unknown => null,
                    SPTPaletteFileType.GPL => GPLSerializer.Deserialize(filename),
                    _ => null,
                };
        }
    }
}