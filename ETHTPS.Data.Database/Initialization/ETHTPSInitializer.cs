using ETHTPS.Data.Core.Database.Relational;

namespace ETHTPS.Data.Integrations.MSSQL.Initialization
{
    public class ETHTPSInitializer : RelationalDatabaseInitializer<EthtpsContext>
    {
        public ETHTPSInitializer(EthtpsContext database) : base(database)
        {
        }

        public override bool CheckConnection()
        {
            throw new NotImplementedException();
        }

        public override bool CheckPermissions()
        {
            throw new NotImplementedException();
        }

        public override bool CheckSchemas()
        {
            throw new NotImplementedException();
        }

        public override bool CheckTables()
        {
            throw new NotImplementedException();
        }

        public override void CreateSchemas()
        {
            throw new NotImplementedException();
        }

        public override void CreateTables()
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void InitializeValues()
        {
            throw new NotImplementedException();
        }
    }
}
