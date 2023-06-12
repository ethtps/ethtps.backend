namespace ETHTPS.Configuration
{
    public interface IEnvironmentProvider
    {
        IEnumerable<string>? GetEnvironments();
        void AddEnvironments(params string[] environments);
    }
}
