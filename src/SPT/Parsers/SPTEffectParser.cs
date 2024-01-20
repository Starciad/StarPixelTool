using System.Collections.Generic;
using System.Linq;
using System;

namespace SPT.Core.Effects
{
    internal static class SPTEffectParser
    {
        internal static SPTEffect ParseEffect(string effect)
        {
            string effectName = string.Empty;
            List<string> effectParams = [];

            int openParenIndex = effect.IndexOf('(');
            if (openParenIndex != -1)
            {
                effectName = effect[..openParenIndex];
                effectParams = effect.Substring(openParenIndex + 1, effect.Length - openParenIndex - 2).Split(',').Select(param => param.Trim()).ToList();
            }
            else
            {
                effectName = effect;
            }

            return default;
        }
    }
}
