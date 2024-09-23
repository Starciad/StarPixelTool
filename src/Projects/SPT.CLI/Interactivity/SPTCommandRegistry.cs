using SPT.CLI.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT.CLI.Interactivity
{
    internal class SPTCommandRegistry
    {
        private readonly Dictionary<string, SPTCommand> _commands = [];

        public void RegisterCommand(SPTCommand command)
        {
            this._commands[command.Name] = command;
            foreach (string alias in command.Aliases)
            {
                this._commands[alias] = command;
            }
        }

        public SPTCommand GetCommand(string name)
        {
            return this._commands.TryGetValue(name, out SPTCommand command) ? command : null;
        }

        public void DisplayHelp()
        {
            Console.WriteLine("Below you can find a detailed list containing all the commands and arguments that can be used in the program.");
            Console.WriteLine();

            foreach (SPTCommand command in this._commands.Values.Distinct())
            {
                SPTTerminal.ApplyColor(ConsoleColor.Green, $"> --{command.Name} ({string.Join(", ", command.Aliases.Select(x => x))}): ");
                Console.Write($"{command.Description}");
                Console.WriteLine();
            }
        }
    }
}
