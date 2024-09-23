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
            Console.WriteLine("Available commands:");
            foreach (SPTCommand command in this._commands.Values.Distinct())
            {
                Console.WriteLine($"{command.Name}: {command.Description}");
            }
        }
    }
}
