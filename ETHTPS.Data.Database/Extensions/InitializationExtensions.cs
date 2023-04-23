using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETHTPS.Data.Integrations.MSSQL.Extensions
{
    public static class InitializationExtensions
    {
        public static void EnsureTablesCreated<TContext>(this TContext context, string connectionString) where TContext : DbContext
        {
            using (var connection = context.Database.GetDbConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                foreach (var property in typeof(TContext).GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)))
                {
                    var entityType = property.PropertyType.GetGenericArguments()[0];
                    var schema = entityType.GetCustomAttributes(typeof(TableAttribute), false).Cast<TableAttribute>().FirstOrDefault()?.Schema ?? "dbo";
                    var tableName = entityType.Name;
                    var tableExistsQuery = $@"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{schema}' AND TABLE_NAME = '{tableName}'";
                    var tableExistsCommand = connection.CreateCommand();
                    tableExistsCommand.CommandText = tableExistsQuery;
                    var tableExists = (int)(tableExistsCommand.ExecuteScalar()??0) > 0;

                    if (!tableExists)
                    {
                        // Check if the user has permission to create tables
                        var userHasPermissionToCreateTablesQuery = "SELECT HAS_PERMS_BY_NAME(NULL, NULL, 'CREATE TABLE')";
                        var userHasPermissionToCreateTablesCommand = connection.CreateCommand();
                        userHasPermissionToCreateTablesCommand.CommandText = userHasPermissionToCreateTablesQuery;
                        var userHasPermissionToCreateTables = (int)(userHasPermissionToCreateTablesCommand.ExecuteScalar()??0) > 0;

                        if (userHasPermissionToCreateTables)
                        {
                            // Create table
                            var createTableScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db", $"init_{entityType.Name}.sql");
                            var createTableScript = File.ReadAllText(createTableScriptPath);
                            var createTableCommand = connection.CreateCommand();
                            createTableCommand.CommandText = createTableScript;
                            createTableCommand.ExecuteNonQuery();

                            // Check if there is a populate script and run it
                            var populateScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db", $"populate_{entityType.Name}.sql");

                            if (File.Exists(populateScriptPath))
                            {
                                var populateScript = File.ReadAllText(populateScriptPath);
                                var populateCommand = connection.CreateCommand();
                                populateCommand.CommandText = populateScript;
                                populateCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("The current user does not have permission to create tables");
                        }
                    }
                }
            }
        }
    }
}
