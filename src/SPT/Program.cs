using SPT.Commands;

using System;
using System.Text;
using System.CommandLine;

namespace SPT
{
    internal static class Program
    {
        [MTAThread]
        private static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            RootCommand rootCommand = new("Transform images into Pixel Art with various filters and effects.");
            SPTCommandBuilder.Initialize(rootCommand);

            return rootCommand.Invoke(args);
        }
    }
}