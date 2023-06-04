using ETHTPS.Data.Core.Database;
using ETHTPS.Data.Core.Database.Relational;
using ETHTPS.Data.Core.Models.DataEntries;

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

        public async Task TryCreateNewBlockSummaryAsync(TPSGPSInfo info)
        {
            if (info.TransactionHashes?.Length == 0 ||
                string.IsNullOrWhiteSpace(info.Provider) ||
                info.TransactionCount == 0 ||
                info.Date == default)
            {
                return;
            }
            await CreateNewBlockSummaryAsync(info.Provider, info.BlockNumber, info.TransactionCount, info.Date, info.GasUsed, info.TransactionHashes);
        }

        public async Task CreateNewBlockSummaryAsync(string providerName, int blockNumber, int transactionCount, DateTime date, int? gasUsed, params string[]? transactionHashes)
        {
            await Database.OpenConnectionAsync();
            var transactionHashesString = transactionHashes != null ? string.Join(",", transactionHashes) : null;
            var parameters = new[]
            {
                new SqlParameter("@ProviderName", providerName),
                new SqlParameter("@BlockNumber", blockNumber),
                new SqlParameter("@TransactionCount", transactionCount),
                new SqlParameter("@Date", date),
                new SqlParameter("@GasUsed", (object?)gasUsed ?? DBNull.Value),
                new SqlParameter("@TransactionHashes", (object?)transactionHashesString ?? DBNull.Value)
            };

            await Database.ExecuteSqlRawAsync("EXEC [BlockInfo].[CreateNewBlockSummary] @ProviderName, @BlockNumber, @TransactionCount, @Date, @GasUsed, @TransactionHashes", parameters);
        }
    }
}
