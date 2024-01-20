using SkiaSharp;

using System;

namespace SPT.Core.Colors
{
    /// <summary>
    /// Provides utility methods for working with <see cref="SKColor"/> objects.
    /// </summary>
    public static class SPTColorMath
    {
        /// <summary>
        /// Calculates the color difference between two <see cref="SKColor"/> objects.
        /// </summary>
        /// <param name="color1">The first <see cref="SKColor"/>.</param>
        /// <param name="color2">The second <see cref="SKColor"/>.</param>
        /// <returns>The Euclidean distance between the two colors in RGB space.</returns>
        public static double Difference(SKColor color1, SKColor color2)
        {
            double deltaR = color1.Red - color2.Red;
            double deltaG = color1.Green - color2.Green;
            double deltaB = color1.Blue - color2.Blue;

            return Math.Sqrt((deltaR * deltaR) + (deltaG * deltaG) + (deltaB * deltaB));
        }

        public static int GetIntensityDifference(SKColor color1, SKColor color2)
        {
            int intensity1 = (int)(color1.Red * 0.3 + color1.Green * 0.59 + color1.Blue * 0.11);
            int intensity2 = (int)(color2.Red * 0.3 + color2.Green * 0.59 + color2.Blue * 0.11);

            return Math.Abs(intensity1 - intensity2);
        }
    }
}
