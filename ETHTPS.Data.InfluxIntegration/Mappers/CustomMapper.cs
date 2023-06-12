
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

namespace ETHTPS.Data.Integrations.InfluxIntegration.Mappers
{
    /// <summary>
    /// A custom mapper for Influx objects since the library seems to be broken.
    /// </summary>
    /// <seealso cref="InfluxDB.Client.IDomainObjectMapper" />
    public sealed class CustomMapper : IDomainObjectMapper
    {
        public T ConvertToEntity<T>(FluxRecord fluxRecord)
        {
            var instance = (T?)Activator.CreateInstance(typeof(T)) ?? throw new Exception($"Could not initialize an instance of {typeof(T).Name}. Make sure it has a public parameterless constructor.");
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                if (Attribute.IsDefined(property, typeof(Column)))
                {
                    var attribute = (Column?)Attribute.GetCustomAttribute(property, typeof(Column));
                    if (attribute == null)
                        continue;
                    var key = attribute.Name;
                    object? value = null;
                    if (attribute.IsTimestamp)
                    {
                        value = fluxRecord.GetTimeInDateTime();
                    }
                    else
                    {
                        var target = fluxRecord.Values.FirstOrDefault(x => x.Key == key || x.Value.ToString() == key);
                        if (target.Value != null && target.Key != null)
                        {
                            value = fluxRecord.GetValue();
                        }
                    }
                    if (value != null)
                    {
                        var convertedValue = Convert.ChangeType(value, property.PropertyType);
                        property.SetValue(instance, convertedValue);
                    }
                }
            }

            return instance;
        }


        public object ConvertToEntity(FluxRecord fluxRecord, Type type)
        {
            throw new NotImplementedException();
        }

        public PointData ConvertToPointData<T>(T entity, WritePrecision precision)
        {
            throw new NotImplementedException();
        }
    }
}
