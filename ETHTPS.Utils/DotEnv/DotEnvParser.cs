using System.Reflection;

namespace ETHTPS.Utils.DotEnv
{
    public class DotEnvParser
    {
        private readonly Dictionary<string, string> _envVars;

        public DotEnvParser(string path =
#if DEBUG
            ".env.development"
#else
            ".env"
#endif
        )
        {
            path = GetAbsolutePath(path);
            if (!File.Exists(path)) throw new FileNotFoundException(path);

            _envVars = new Dictionary<string, string>();
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    continue;
                }

                var parts = line.Split("=", 2);
                if (parts.Length != 2)
                {
                    throw new FormatException($"Invalid .env format: {line}");
                }

                _envVars[parts[0]] = parts[1];
            }
        }

        public string Get(string key) => Get<string>(key);

        public T Get<T>(string key)
        {
            if (!_envVars.TryGetValue(key, out var value))
            {
                throw new KeyNotFoundException($"Key not found: {key}");
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (FormatException)
            {
                throw new FormatException($"Invalid value format for key {key}: {value}");
            }
        }

        private static string GetAbsolutePath(string filePath)
        {
            if (!Path.IsPathRooted(filePath))
            {
                var assemblyLocation = Assembly.GetExecutingAssembly().Location;
                var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
                if (assemblyDirectory != null)
                {
                    filePath = Path.Combine(assemblyDirectory, filePath);
                }
            }

            return Path.GetFullPath(filePath);
        }
    }
}
