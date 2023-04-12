using ETHTPS.Utils.DotEnv;
using System;
using static ETHTPS.Data.Core.Constants.EnvironmentVariables;

namespace ETHTPS.Data.Core.Extensions
{
    public static class EnvironmentExtensions
    {
        private static DotEnvParser _parser = new(".env.data.development");
        public static string GetEnvVarValue(string name)
        {
            var env = GetEnvironmentVariableFromAnywherePriorityWiseOrThrow(ENV);
            if (string.IsNullOrWhiteSpace(env))
                throw new ArgumentNullException(ENV);
            string? result = null;
            result = env switch
            {
                "DEVELOPMENT" => GetEnvironmentVariableFromAnywherePriorityWiseOrThrow($"{name}"),
                _ => throw new ArgumentOutOfRangeException(ENV)
            };
            if (string.IsNullOrEmpty(result))
                throw new Exception($"{name} not defined");
            else return result;
        }

        /// <summary>
        /// Just get it from anywhere, I don't really care
        /// </summary>
        private static string GetEnvironmentVariableFromAnywherePriorityWiseOrThrow(string name) => 
            Environment.GetEnvironmentVariable(name) ?? 
            Environment.GetEnvironmentVariable(name, 
                EnvironmentVariableTarget.Process) ?? 
            Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User) ?? 
            Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine) ??
            _parser.Get(name) ?? throw new ArgumentNullException(name);
    }
}
