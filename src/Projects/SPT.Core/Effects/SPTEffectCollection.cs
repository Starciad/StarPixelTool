using SPT.Core.Effects.Common;

using System;
using System.Collections.Generic;

namespace SPT.Core.Effects
{
    public static class SPTEffectCollection
    {
        private static readonly SPTEffect[] definedEffects =
        [
            new SPTOutlineEffect(),
        ];

        public static SPTEffect GetEffectByName(string name)
        {
            return Array.Find(definedEffects, x => x.Name == name);
        }
    }
}
