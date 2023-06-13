using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Configuration.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string? GetFirstConfigurationString(this IDBConfigurationProvider provider, string key) => provider.GetConfigurationStrings(key)?.FirstOrDefault()?.Value;

        public static string? GetFirstConfigurationStringForCurrentEnvironment(this IDBConfigurationProvider provider, string key, string microservice) => provider.GetConfigurationStringsForMicroservice(microservice)?.FirstOrDefault(x => x.Name == key)?.Value;

        public static void ConfigureEntityPrimaryKey<T>(this ModelBuilder modelBuilder) where T : class
        {
            var entityType = modelBuilder.Model.FindEntityType(typeof(T));
            if (entityType != null)
            {
                var primaryKey = entityType.FindPrimaryKey() ?? throw new InvalidOperationException($"Cannot configure primary key for entity type '{entityType.Name}' because it has no primary key defined.");
                modelBuilder.Entity<T>().HasKey(primaryKey.Properties.Select(x => x.Name).ToArray());

                foreach (var foreignKey in entityType.GetForeignKeys())
                {
                    var foreignKeyName = foreignKey.PrincipalKey.GetName();
                    var clrType = foreignKey.DependentToPrincipal?.ClrType;
                    if (clrType != null)
                    {
                        modelBuilder.Entity<T>().HasOne(clrType)
                            .WithMany(foreignKey.PrincipalToDependent?.ClrType.FullName)
                            .HasForeignKey(foreignKey.Properties.Select(x => x.Name).ToArray())
                            .HasConstraintName($"FK_{entityType.GetTableName()}_{foreignKeyName}");
                    }
                }
            }
        }
    }
}
