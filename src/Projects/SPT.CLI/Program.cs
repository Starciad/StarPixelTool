using SPT.Commands;

using System;
using System.CommandLine;
using System.Text;

namespace SPT.CLI
{
    internal static class Program
    {
        [MTAThread]
        private static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            return Initialize(args);
        }

        private static int Initialize(string[] args)
        {
            SPTSettingsManager.Initialize();

            RootCommand rootCommand = new("SPT is a utility that allows you to pixelate images, allowing you to apply dozens of different filters and settings.");
            SPTCommandBuilder.Initialize(rootCommand);

            return rootCommand.Invoke(args);
        }
    }
}