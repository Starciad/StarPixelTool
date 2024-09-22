using System;
using System.Collections.Generic;

namespace SPT.CLI.Utilities
{
    internal sealed class SPTArgumentParser
    {
        private readonly Dictionary<string, string> arguments = [];
        private readonly HashSet<string> flags = [];

        internal SPTArgumentParser(string[] args)
        {
            ParseArgs(args);
        }

        // ============================================== //

        public bool HasOption(string option)
        {
            return this.arguments.ContainsKey(option);
        }

        public string GetOption(string option)
        {
            return this.arguments.TryGetValue(option, out string value) ? value : null;
        }

        public bool HasFlag(string flag)
        {
            return this.flags.Contains(flag);
        }

        public static void ShowHelp(string programName, string description, Dictionary<string, string> availableCommands)
        {
            Console.WriteLine($"{programName}\n{description}\n\nComandos Disponíveis:");

            foreach (KeyValuePair<string, string> command in availableCommands)
            {
                Console.WriteLine($"--{command.Key}: {command.Value}");
            }
        }

        // ============================================== //

        private void ParseArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                string value = i + 1 < args.Length && !args[i + 1].StartsWith('-') ? args[++i] : null;

                if (arg.StartsWith("--"))
                {
                    // Option
                    AddArg(arg[2..], value);
                }
                else if (arg.StartsWith("-"))
                {
                    // Alias
                    AddArg(arg[1..], value);
                }
                else
                {
                    _ = this.flags.Add(arg);
                }
            }

            void AddArg(string key, string value)
            {
                this.arguments.Add(key, value);
            }
        }
    }
}
