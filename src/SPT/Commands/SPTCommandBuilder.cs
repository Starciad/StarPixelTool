using SPT.Terminal;

using System;
using System.CommandLine;

namespace SPT.Commands
{
    internal static partial class SPTCommandBuilder
    {
        internal static void Initialize(RootCommand root)
        {
            InitializeCompatibilityCommand(root);
            InitializeFilesCommand(root);
            InitializePalettesCommand(root);
            InitializeTransformCommand(root);
        }
    }
}