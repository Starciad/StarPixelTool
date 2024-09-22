using SkiaSharp;

using SPT.Core.Colors;
using SPT.Core.Extensions;

using System.Linq;

namespace SPT.Core.Effects.Common
{
    public sealed class SPTOutlineEffect : SPTEffect
    {
        private readonly SKColor color = SKColors.Black;
        private readonly float power = 256f;

        protected override void OnBuild()
        {
            this.Name = "outline";
            this.Description = "";
        }

        protected override void OnApply(SKBitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

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

                    if (intensityDifference > this.power)
                    {
                        bitmap.SetPixel(x, y, this.color);
                    }
                }
            }
        }
    }
}