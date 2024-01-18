using SkiaSharp;

namespace SPT.Core.Extensions
{
    internal static class SKBitmapExtensions
    {
        internal static SKColor GetAverageColorInArea(this SKBitmap bitmap, int x, int y, int areaSize)
        {
            int count = 0;
            int totalR = 0, totalG = 0, totalB = 0;

            for (int i = -1; i < areaSize; i++)
            {
                for (int j = -1; j < areaSize; j++)
                {
                    int xPivot = x + i;
                    int yPivot = y + j;

                    if (bitmap.IsWithinBitmapBounds(xPivot, yPivot))
                    {
                        SKColor pixelColor = bitmap.GetPixel(xPivot, yPivot);
                        totalR += pixelColor.Red;
                        totalG += pixelColor.Green;
                        totalB += pixelColor.Blue;

                        count++;
                    }
                }
            }

            if (count > 0)
            {
                byte averageR = (byte)(totalR / count);
                byte averageG = (byte)(totalG / count);
                byte averageB = (byte)(totalB / count);

                return new SKColor(averageR, averageG, averageB, 255);
            }

            return SKColors.Empty;
        }

        private static bool IsWithinBitmapBounds(this SKBitmap bitmap, int x, int y)
        {
            return x >= 0 && x < bitmap.Width &&
                   y >= 0 && y < bitmap.Height;
        }
    }
}
