using SkiaSharp;

using SPT.Core.Extensions;

using System;
using System.IO;

namespace SPT.Core
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    public sealed class SPTPixelator(FileStream inputFile, FileStream outputFile) : IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        public required int PixelateFactor
        {
            get => this.pixelateFactor;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Pixelate factor must be greater than 0.");
                }

                this.pixelateFactor = value;
            }
        }

        private SKBitmap bitmapInput;
        private SKBitmap bitmapOutput;

        private int widthInput;
        private int widthOutput;
        private int heightInput;
        private int heightOutput;

        private bool disposedValue;

        private int pixelateFactor;

        #region System
        /// <summary>
        /// Initializes the pixelation process on the input image, creating a pixelated version.
        /// </summary>
        public void InitializePixelation()
        {
            InitializeFiles();
            StartPixelationProcess();
        }

        /// <summary>
        /// Exports the pixelated image to the specified output file.
        /// </summary>
        /// <exception cref="IOException">Thrown if the export operation fails. Check the output file path and try again.</exception>
        public void ExportPixelatedImage()
        {
            if (!this.bitmapOutput.Encode(outputFile, SKEncodedImageFormat.Png, default))
            {
                throw new IOException("Failed to export the pixelated image. Check the output file path and try again.");
            }
        }
        #endregion

        #region Processing
        private void InitializeFiles()
        {
            this.bitmapInput = SKBitmap.Decode(inputFile);
            this.widthInput = this.bitmapInput.Width;
            this.heightInput = this.bitmapInput.Height;

            this.widthOutput = this.widthInput / this.pixelateFactor;
            this.heightOutput = this.heightInput / this.pixelateFactor;
            this.bitmapOutput = new SKBitmap(this.widthOutput, this.heightOutput);
        }
        private void StartPixelationProcess()
        {
            ApplyPixelation();
        }
        private void ApplyPixelation()
        {
            int inputPosX = 0;
            int inputPosY = 0;

            for (int y = 0; y < this.heightOutput; y++)
            {
                for (int x = 0; x < this.widthOutput; x++)
                {
                    SKColor color = this.bitmapInput.GetAverageColorInArea(inputPosX, inputPosY, 3);

                    this.bitmapOutput.SetPixel(x, y, color);
                    inputPosX += this.pixelateFactor;
                }

                inputPosX = 0;
                inputPosY += this.pixelateFactor;
            }
        }
        #endregion

        #region Dispose
        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    ((IDisposable)this.bitmapInput).Dispose();
                    ((IDisposable)this.bitmapOutput).Dispose();

                    inputFile.Close();
                    outputFile.Close();
                }

                this.disposedValue = true;
            }
        }
        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
