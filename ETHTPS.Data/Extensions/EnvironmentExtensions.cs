using System;
using static ETHTPS.Data.Core.Constants.EnvironmentVariables;

namespace ETHTPS.Data.Core.Extensions
{
    public static class EnvironmentExtensions
    {
        public static string GetEnvVarValue(string name)
        {
            var env = Environment.GetEnvironmentVariable(ENV, EnvironmentVariableTarget.Machine);
            if (string.IsNullOrWhiteSpace(env))
                throw new ArgumentNullException(ENV);
            string? result = null;
            switch (env)
            {
                case "DEVELOPMENT":
                    result = Environment.GetEnvironmentVariable($"{name}_{env}", EnvironmentVariableTarget.Machine);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(ENV);
            }
            if (string.IsNullOrEmpty(result))
                throw new Exception($"{name} not defined");
            else return result;
        }
    }
}
