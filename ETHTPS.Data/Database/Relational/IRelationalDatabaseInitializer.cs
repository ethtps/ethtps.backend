namespace ETHTPS.Data.Core.Database.Relational
{
    /// <summary>
    /// A class for initializing a relational database.
    /// </summary>
    public interface IRelationalDatabaseInitializer<TDatabase> where TDatabase : IRelationalDatabase
    {
        public bool CheckConnection();
        public bool CheckPermissions();
        public bool CheckTables();
        public bool CheckSchemas();
        public void CreateSchemas();
        public void CreateTables();
        public void InitializeValues();
    }
}
