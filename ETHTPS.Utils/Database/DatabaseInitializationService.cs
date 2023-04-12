using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace ETHTPS.Utils.Database
{
    public class DatabaseInitializationService : IDatabaseInitializationService
    {
        private readonly ILogger<DatabaseInitializationService> _logger;

        public DatabaseInitializationService(ILogger<DatabaseInitializationService> logger)
        {
            _logger = logger;
        }

        public void InitializeDatabase(string directoryName, string connectionString)
        {
            var tableNames = GetTableNames(directoryName);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var tableName in tableNames)
                {
                    var createSqlFile = Path.Combine(directoryName, $"{tableName}_create.sql");
                    var initSqlFile = Path.Combine(directoryName, $"{tableName}_init.sql");

                    if (File.Exists(createSqlFile))
                    {
                        if (!HasCreatePermission(connection, tableName))
                        {
                            _logger.LogError($"User does not have permission to create table {tableName}.");
                            continue;
                        }

                        ExecuteScriptFile(connection, createSqlFile);
                        _logger.LogInformation($"Table {tableName} created.");
                    }

                    if (File.Exists(initSqlFile))
                    {
                        ExecuteScriptFile(connection, initSqlFile);
                        _logger.LogInformation($"Table {tableName} initialized.");
                    }
                }
            }
        }

        private IEnumerable<string> GetTableNames(string directoryName)
        {
            var tableNames = new List<string>();
            var fileNames = Directory.GetFiles(directoryName, "*.sql");

            foreach (var fileName in fileNames)
            {
                var tableName = Path.GetFileNameWithoutExtension(fileName);
                tableNames.Add(tableName);
            }

            return tableNames.Distinct();
        }

        private bool HasCreatePermission(SqlConnection connection, string tableName)
        {
            using (var command = new SqlCommand($"SELECT HAS_PERMS_BY_NAME('{tableName}', 'OBJECT', 'CREATE TABLE')", connection))
            {
                var hasPermission = (int)command.ExecuteScalar() == 1;
                return hasPermission;
            }
        }

        private void ExecuteScriptFile(SqlConnection connection, string fileName)
        {
            var script = File.ReadAllText(fileName);

            using (var command = new SqlCommand(script, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}