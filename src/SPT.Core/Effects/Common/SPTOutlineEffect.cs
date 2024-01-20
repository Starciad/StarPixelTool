using SkiaSharp;

using SPT.Core.Colors;
using SPT.Core.Extensions;

using System.Linq;

namespace SPT.Core.Effects.Common
{
    public sealed class SPTOutlineEffect : SPTEffect
    {
        protected override void OnBuild()
        {
            this.Name = "";
            this.Description = "";
        }

        protected override void OnApply(SKBitmap bitmap, object[] parameters)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            float power = (float)parameters[0];
            SKColor color = (SKColor)parameters[1];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SKColor cColor = bitmap.GetPixel(x, y);
                    SKColor[] colors = bitmap.GetNeighboringColors(x, y).Select(x => x.color).ToArray();

                    int intensityDifference = 0;
                    for (int i = 0; i < colors.Length; i++)
                    {
                        intensityDifference += SPTColorMath.GetIntensityDifference(cColor, colors[i]);
                    }

                    if (intensityDifference > power)
                    {
                        bitmap.SetPixel(x, y, color);
                    }
                }
            }
        }
    }
}