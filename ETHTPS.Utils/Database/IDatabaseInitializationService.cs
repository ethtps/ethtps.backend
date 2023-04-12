namespace ETHTPS.Utils.Database
{
    public interface IDatabaseInitializationService
    {
        public void InitializeDatabase(string directoryName, string connectionString);
    }
}
