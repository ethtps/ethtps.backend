using System.Data;

using ETHTPS.Data.Core.Database;
using ETHTPS.Data.Core.Database.Relational;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Data.Integrations.MSSQL
{
    public abstract class ETHTPSContextBase : ContextBase<EthtpsContext>, IRelationalDatabase
    {
        public ETHTPSContextBase()
        {

        }

        public ETHTPSContextBase(DbContextOptions<EthtpsContext> options)
            : base(options)
        {

        }

        public async Task<int> InsertNewExperimentAsync(
          int projectId,
          string experimentName,
          string experimentDescription,
          string targetName,
          DateTime startDate,
          DateTime endDate,
          bool enabled,
          int displayToNPeopleBeforeEnd,
          int considerFinishedAfterTimeoutSeconds)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[ABTesting].[InsertNewExperiment]";

                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter("@ProjectID", projectId),
                new SqlParameter("@ExperimentName", experimentName),
                new SqlParameter("@ExperimentDescription", experimentDescription),
                new SqlParameter("@TargetName", targetName),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate),
                new SqlParameter("@Enabled", enabled),
                new SqlParameter("@DisplayToNPeopleBeforeEnd", displayToNPeopleBeforeEnd),
                new SqlParameter("@ConsiderFinishedAfterTimeoutSeconds", considerFinishedAfterTimeoutSeconds)
                });

                await Database.OpenConnectionAsync();
                var result = await command.ExecuteNonQueryAsync();
                return result;
            }
        }

        public async Task<int> DeleteAllJobsAsync()
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[HangFire].[DeleteAllJobs]";

                await Database.OpenConnectionAsync();
                var result = await command.ExecuteNonQueryAsync();
                return result;
            }
        }

        public async Task<int> InsertNewExperimentTargetAsync(
            string targetTypeName,
            string targetTypeValue)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[ABTesting].[InsertNewExperimentTarget]";

                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter("@TargetTypeName", targetTypeName),
                new SqlParameter("@TargetTypeValue", targetTypeValue)
                });

                await Database.OpenConnectionAsync();
                var result = await command.ExecuteNonQueryAsync();
                return result;
            }
        }

        public async Task<int> InsertNewDataUpdaterAsync(
        string typeName,
        int providerId)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "[DataUpdaters].[InsertNewDataUpdater]";

                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter("@TypeName", typeName),
                new SqlParameter("@ProviderID", providerId)
                });

                await Database.OpenConnectionAsync();
                var result = await command.ExecuteNonQueryAsync();
                return result;
            }
        }

        public async Task UpsertRPCEndpoints(
          string? description,
          string? authType,
          string? apiKey,
          string network,
          string endpoints,
          string providerName)
        {
            description ??= string.Empty;
            authType ??= string.Empty;
            apiKey ??= string.Empty;
            var parameters = new[]
            {
            new SqlParameter("@Description", SqlDbType.VarChar) { Value = description },
            new SqlParameter("@AuthType", SqlDbType.VarChar) { Value = authType },
            new SqlParameter("@ApiKey", SqlDbType.VarChar) { Value = apiKey },
            new SqlParameter("@Network", SqlDbType.VarChar) { Value = network },
            new SqlParameter("@Endpoints", SqlDbType.VarChar) { Value = endpoints },
            new SqlParameter("@ProviderName", SqlDbType.VarChar) { Value = providerName }
        };

            await Database.ExecuteSqlRawAsync("EXEC [RPC].[UpsertRPCEndpoints] @Description, @AuthType, @ApiKey, @Network, @Endpoints, @ProviderName", parameters);
        }
    }
}
