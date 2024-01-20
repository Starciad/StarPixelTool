using SkiaSharp;

namespace SPT.Core.Effects
{
    public abstract class SPTEffect
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public SPTEffect()
        {
            OnBuild();
        }

        public void Apply(SKBitmap bitmap, params object[] parameters)
        {
            OnApply(bitmap, parameters);
        }

        protected abstract void OnBuild();
        protected abstract void OnApply(SKBitmap bitmap, object[] parameters);
    }
}
