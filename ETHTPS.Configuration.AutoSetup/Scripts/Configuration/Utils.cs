namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration
{
    public static class Utils
    {
        /// <summary>
        /// https://stackoverflow.com/a/35824406
        /// </summary>
        public static DirectoryInfo? TryGetSolutionDirectoryInfo(string? currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
    }
}
