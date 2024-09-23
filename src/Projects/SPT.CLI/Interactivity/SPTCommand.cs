using System;
using System.Collections.Generic;

namespace SPT.CLI.Interactivity
{
    internal class SPTCommand
    {
        public string Name { get; }
        public List<string> Aliases { get; } = [];
        public string Description { get; }
        public Action<SPTArgumentParser> Execute { get; }

        public SPTCommand(string name, string description, Action<SPTArgumentParser> execute, params string[] aliases)
        {
            this.Name = name;
            this.Description = description;
            this.Execute = execute;

            if (aliases != null && aliases.Length > 0)
            {
                this.Aliases.AddRange(aliases);
            }
        }
    }
}
