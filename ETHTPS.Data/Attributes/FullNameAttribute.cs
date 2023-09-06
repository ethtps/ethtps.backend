using System;

namespace ETHTPS.Data.Core.Attributes
{
    /// <summary>
    /// Describes the full name of an application/microservice.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class FullNameAttribute : Attribute
    {
        public string FullName { get; }

        public FullNameAttribute(string fullName)
        {
            FullName = fullName;
        }
    }
}
