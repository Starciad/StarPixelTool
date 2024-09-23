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
        public static double DifferenceRGB(SKColor color1, SKColor color2)
        {
            double deltaR = color1.Red - color2.Red;
            double deltaG = color1.Green - color2.Green;
            double deltaB = color1.Blue - color2.Blue;

            return Math.Sqrt((deltaR * deltaR) + (deltaG * deltaG) + (deltaB * deltaB));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns></returns>
        public static double DifferenceHSL(SKColor color1, SKColor color2)
        {
            color1.ToHsl(out float h1, out float s1, out float l1);
            color2.ToHsl(out float h2, out float s2, out float l2);

            float deltaH = h1 - h2;
            float deltaS = s1 - s2;
            float deltaL = l1 - l2;

            return Math.Sqrt((deltaH * deltaH) + (deltaS * deltaS) + (deltaL * deltaL));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns></returns>
        public static double DifferenceHSV(SKColor color1, SKColor color2)
        {
            color1.ToHsv(out float h1, out float s1, out float v1);
            color2.ToHsv(out float h2, out float s2, out float v2);

            float deltaH = h1 - h2;
            float deltaS = s1 - s2;
            float deltaV = v1 - v2;

            return Math.Sqrt((deltaH * deltaH) + (deltaS * deltaS) + (deltaV * deltaV));
        }

        public static int GetIntensityDifference(SKColor color1, SKColor color2)
        {
            int intensity1 = (int)((color1.Red * 0.3) + (color1.Green * 0.59) + (color1.Blue * 0.11));
            int intensity2 = (int)((color2.Red * 0.3) + (color2.Green * 0.59) + (color2.Blue * 0.11));

            return Math.Abs(intensity1 - intensity2);
        }
    }
}
