namespace ETHTPS.Data.Core.Database.Relational
{
    public abstract class RelationalDatabaseInitializer<TDatabase> : IRelationalDatabaseInitializer<TDatabase>
        where TDatabase : IRelationalDatabase
    {
        protected readonly TDatabase _database;
        protected RelationalDatabaseInitializer(TDatabase database)
        {
            _database = database;
        }

        public abstract bool CheckConnection();
        public abstract bool CheckPermissions();
        public abstract bool CheckSchemas();
        public abstract bool CheckTables();
        public abstract void CreateSchemas();
        public abstract void CreateTables();
        public abstract void Initialize();
        public abstract void InitializeValues();
    }
}
