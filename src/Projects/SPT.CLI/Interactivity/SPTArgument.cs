using System;

namespace SPT.CLI.Interactivity
{
    internal sealed class SPTArgument
    {
        internal string Name { get; private set; }
        internal string Alias { get; private set; }
        internal string Description { get; private set; }
        internal Type ValueType { get; private set; }

        internal SPTArgument(string name, string alias, string description, Type valueType)
        {
            this.Name = name;
            this.Alias = alias;
            this.Description = description;
            this.ValueType = valueType;
        }
    }
}
