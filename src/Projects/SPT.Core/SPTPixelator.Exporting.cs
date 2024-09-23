using SkiaSharp;

using System.IO;

namespace SPT.Core
{
    public sealed partial class SPTPixelator
    {
        /// <summary>
        /// Exports the pixelated image to the specified output file.
        /// </summary>
        /// <exception cref="IOException">Thrown if the export operation fails. Check the output file path and try again.</exception>
        public void ExportPixelatedImage()
        {
            if (!this.bitmapOutput.Encode(this.outputFileStream, SKEncodedImageFormat.Png, default))
            {
                throw new IOException("Failed to export the pixelated image. Check the output file path and try again.");
            }
        }
    }
}
