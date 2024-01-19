using SkiaSharp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPT.Core.Palettes.Serializers
{
    /// <summary>
    /// Provides methods for serializing and deserializing <see cref="SPTPalette"/> objects to and from GIMP palette (.gpl) files.
    /// </summary>
    public static class GPLSerializer
    {
        private static readonly char[] separator = [' ', '\t'];

        /// <summary>
        /// Deserializes a GIMP palette (.gpl) file and returns an <see cref="SPTPalette"/> object.
        /// </summary>
        /// <param name="filename">The path to the GIMP palette file (.gpl).</param>
        /// <returns>An <see cref="SPTPalette"/> object representing the deserialized palette.</returns>
        /// <exception cref="ArgumentException">Thrown when the path to the file is null or empty, or when the specified file is not a .GPL file.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the GPL file for deserialization is not found.</exception>
        public static SPTPalette Deserialize(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("The path to the file is null or empty.", nameof(filename));
            }

            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("Unable to find GPL file for deserialization.", filename);
            }

            if (!Path.GetExtension(filename).Equals(".gpl", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ArgumentException("The specified file is not a .GPL file.", nameof(filename));
            }

            string[] lines = File.ReadAllLines(filename);

            // Filter out empty lines
            lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            List<SKColor> paletteColors = [];

            foreach (string line in lines)
            {
                if (line.StartsWith("GIMP Palette") || line.StartsWith('#') || line.StartsWith("Name") || line.StartsWith("Columns"))
                {
                    continue;
                }
                else
                {
                    string[] values = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length >= 4)
                    {
                        paletteColors.Add(new SKColor(
                            red: byte.Parse(values[0]),
                            green: byte.Parse(values[1]),
                            blue: byte.Parse(values[2])
                        ));
                    }
                }
            }

            return new SPTPalette([.. paletteColors]);
        }
    }
}
