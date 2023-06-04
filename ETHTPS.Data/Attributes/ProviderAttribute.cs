using System;

namespace ETHTPS.Data.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ProviderAttribute : Attribute
    {
        public ProviderAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
