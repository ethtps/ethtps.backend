namespace ETHTPS.Utils.Database
{
    public interface IDatabaseInitializer
    {
        public void InitializeDatabase(string directoryName, string connectionString);
    }
}
