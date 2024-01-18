using SkiaSharp;

using SPT.Extensions;

using System;
using System.IO;

namespace SPT
{
    internal sealed class PixelArtConverter : IDisposable
    {
        private readonly FileStream fileInput;
        private readonly FileStream fileOutput;

        private readonly SKBitmap bitmapInput;
        private readonly SKBitmap bitmapOutput;

        private readonly int widthInput;
        private readonly int widthOutput;

        private readonly int heightInput;
        private readonly int heightOutput;

        private readonly int pixelScale;
        //private readonly int tolerance;

        private bool disposedValue;

        internal PixelArtConverter(FileInfo inputFile, FileInfo outputFile, int pixelScale, int tolerance)
        {
            this.fileInput = inputFile.Open(FileMode.Open);
            this.fileOutput = outputFile.Open(FileMode.OpenOrCreate);

            this.pixelScale = pixelScale;
            // this.tolerance = tolerance;

            this.bitmapInput = SKBitmap.Decode(this.fileInput);
            this.widthInput = this.bitmapInput.Width;
            this.heightInput = this.bitmapInput.Height;

            this.widthOutput = this.widthInput / pixelScale;
            this.heightOutput = this.heightInput / pixelScale;
            this.bitmapOutput = new SKBitmap(this.widthOutput, this.heightOutput);
        }

        // ================================= //

        internal void Start()
        {
            ApplyPixelEffects();
        }
        internal void Export()
        {
            _ = this.bitmapOutput.Encode(this.fileOutput, SKEncodedImageFormat.Png, default);
        }

        // ================================= //

        private void ApplyPixelEffects()
        {
            int inputPosX = 0;
            int inputPosY = 0;

            for (int y = 0; y < this.heightOutput; y++)
            {
                for (int x = 0; x < this.widthOutput; x++)
                {
                    SKColor color = this.bitmapInput.GetAverageColorInArea(inputPosX, inputPosY, 3);

                    this.bitmapOutput.SetPixel(x, y, color);
                    inputPosX += this.pixelScale;
                }

                inputPosX = 0;
                inputPosY += this.pixelScale;
            }
        }

        // ================================= //

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    ((IDisposable)this.bitmapInput).Dispose();
                    ((IDisposable)this.bitmapOutput).Dispose();

                    this.fileInput.Close();
                    this.fileOutput.Close();
                }

                this.disposedValue = true;
            }
        }
        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
