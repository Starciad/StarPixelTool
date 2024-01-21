using SkiaSharp;

namespace SPT.Core.Effects
{
    /// <summary>
    /// Represents an base class for effects in the SPT.
    /// </summary>
    public abstract class SPTEffect
    {
        /// <summary>
        /// Gets the name of the effect.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets the description of the effect.
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPTEffect"/> class.
        /// </summary>
        public SPTEffect()
        {
            OnBuild();
        }

        public void ApplyEffect(SKBitmap bitmap)
        {
            OnApply(bitmap);
        }

        protected abstract void OnBuild();
        protected abstract void OnApply(SKBitmap bitmap);
    }
}