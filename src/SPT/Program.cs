using SPT.Commands;

using System;
using System.CommandLine;
using System.Text;

namespace SPT
{
    internal static class Program
    {
        [MTAThread]
        private static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            RootCommand rootCommand = new("SPT is a utility that allows you to pixelate images, allowing you to apply dozens of different filters and settings.");
            SPTCommandBuilder.Initialize(rootCommand);

            return rootCommand.Invoke(args);
        }
    }
}