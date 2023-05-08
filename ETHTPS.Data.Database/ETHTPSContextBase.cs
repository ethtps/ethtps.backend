﻿using ETHTPS.Data.Core.Database;
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
    }
}
