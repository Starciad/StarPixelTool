using SkiaSharp;

using SPT.Core.Colors;
using SPT.Core.Enums;

using System;

namespace SPT.Core.Palettes
{
    /// <summary>
    /// Represents a color palette.
    /// </summary>
    /// <remarks>
    /// This class provides functionality for managing and manipulating color palettes.
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SPTPalette"/> class with the specified colors.
    /// </remarks>
    /// <param name="colors">The array of <see cref="SKColor"/> objects representing the color palette.</param>
    public sealed class SPTPalette(params SKColor[] colors)
    {
        /// <summary>
        /// Gets a value indicating whether the color palette is empty.
        /// </summary>
        public bool IsEmpty => this.Size == 0;

        /// <summary>
        /// Gets the size of the color palette.
        /// </summary>
        public uint Size => (uint)colors.Length;

        /// <summary>
        /// Gets the array of <see cref="SKColor"/> objects representing the color palette.
        /// </summary>
        public SKColor[] Colors => colors;

        /// <summary>
        /// Specifies the selected color space for color comparison.
        /// </summary>
        public SPTColorSpaceType SelectedColorSpace { get; set; } = SPTColorSpaceType.RGB;

        /// <summary>
        /// Gets the closest color in the palette to the specified color.
        /// </summary>
        /// <param name="color">The <see cref="SKColor"/> to find the closest match for.</param>
        /// <returns>The closest <see cref="SKColor"/> in the palette.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the color palette is empty.</exception>
        public SKColor GetClosestColor(SKColor color)
        {
            if (this.Size == 0)
            {
                throw new InvalidOperationException("The color palette is empty. Cannot find the closest color.");
            }
            else if (this.Size == 1)
            {
                return this.Colors[0];
            }
            else
            {
                SKColor closestColor = this.Colors[0];
                double minDifference = GetColorDifference(color, closestColor);

                for (int i = 1; i < this.Size; i++)
                {
                    SKColor currentColor = this.Colors[i];
                    double currentDifference = GetColorDifference(color, currentColor);

                    if (currentDifference < minDifference)
                    {
                        minDifference = currentDifference;
                        closestColor = currentColor;
                    }
                }

                return closestColor;
            }
        }

        private double GetColorDifference(SKColor color1, SKColor color2)
        {
            return this.SelectedColorSpace switch
            {
                SPTColorSpaceType.RGB => SPTColorMath.DifferenceRGB(color1, color2),
                SPTColorSpaceType.HSL => SPTColorMath.DifferenceHSL(color1, color2),
                SPTColorSpaceType.HSV => SPTColorMath.DifferenceHSV(color1, color2),
                _ => throw new NotSupportedException("Unsupported color space."),
            };
        }
    }
}
