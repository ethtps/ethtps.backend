using System;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json.Serialization;

namespace ETHTPS.Services.Infrastructure.Serialization
{
    public sealed class NullToEmptyListResolver : DefaultContractResolver
    {
        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
        {
            IValueProvider provider = base.CreateMemberValueProvider(member);

            if (member.MemberType == MemberTypes.Property)
            {
                Type propType = ((PropertyInfo)member).PropertyType;
                if (propType.IsGenericType &&
                    propType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return new EmptyListValueProvider(provider, propType);
                }
            }

            return provider;
        }

        public sealed class EmptyListValueProvider : IValueProvider
        {
            private IValueProvider _innerProvider;
            private object _defaultValue;

            public EmptyListValueProvider(IValueProvider innerProvider, Type listType)
            {
                this._innerProvider = innerProvider;
                _defaultValue = Activator.CreateInstance(listType);
            }

            public void SetValue(object target, object value)
            {
                _innerProvider.SetValue(target, value ?? _defaultValue);
            }

            public object GetValue(object target)
            {
                return _innerProvider.GetValue(target) ?? _defaultValue;
            }
        }
    }
}
