using System.Collections.Generic;

namespace SPT.CLI.Interactivity
{
    internal sealed class SPTArgumentParser
    {
        private readonly Dictionary<string, string> _options = [];

        public SPTArgumentParser(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith('-'))
                {
                    string key = args[i].TrimStart('-');
                    string value = i + 1 < args.Length && !args[i + 1].StartsWith('-') ? args[++i] : null;
                    this._options[key] = value;
                }
            }
        }

        public bool HasOption(string name)
        {
            return this._options.ContainsKey(name);
        }

        public string GetOption(string name)
        {
            return this._options.TryGetValue(name, out string value) ? value : null;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllOptions()
        {
            return this._options;
        }
    }
}
