﻿namespace ETHTPS.API.Security.Core.Policies
{
    public sealed class PolicyDescriptor
    {
        public PolicyDescriptor(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}
