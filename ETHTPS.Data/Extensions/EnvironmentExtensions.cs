using System;

using ETHTPS.Utils.DotEnv;

using static ETHTPS.Data.Core.Constants.EnvironmentVariables;

namespace ETHTPS.Data.Core.Extensions
{
    public static class EnvironmentExtensions
    {
        private static readonly DotEnvParser _parser = new();
        public static string GetEnvVarValue(string name)
        {
            var value = MaybeGetEnvironmentVariable(ETHTPS_ENV) ?? GetEnvironmentVariableFromAnywherePriorityWiseOrThrow(ETHTPS_ENV, _parser);
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(ETHTPS_ENV);
            string? result = null;
            result = value switch
            {
                "DEVELOPMENT" => GetEnvironmentVariableFromAnywherePriorityWiseOrThrow($"{name}", _parser),
                "TESTING" => GetEnvironmentVariableFromAnywherePriorityWiseOrThrow($"{name}", new DotEnvParser(".env.testing")),
                _ => throw new ArgumentOutOfRangeException($"\"{ETHTPS_ENV}\" was \"{value}\"")
            };
            if (string.IsNullOrEmpty(result))
                throw new Exception($"{name} not defined");
            else return result;
        }

        /// <summary>
        /// Just get it from anywhere, I don't really care
        /// </summary>
        private static string GetEnvironmentVariableFromAnywherePriorityWiseOrThrow(string name, DotEnvParser parser) =>
            MaybeGetEnvironmentVariable(name) ??
            parser.Get(name) ??
            throw new ArgumentNullException(name);

        /// <summary>
        /// Just get it from anywhere in the system, I don't really care
        /// </summary>
        private static string? MaybeGetEnvironmentVariable(string name) =>
            Environment.GetEnvironmentVariable(name) ??
            Environment.GetEnvironmentVariable(name,
                EnvironmentVariableTarget.Process) ??
            Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User) ??
            Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
    }
}
