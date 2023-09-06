using ETHTPS.Data.Core;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class ApikeyGroup : IIndexed
{
    public int Id { get; set; }

    public int ApikeyId { get; set; }

    public int GroupId { get; set; }

    public virtual Apikey? Apikey { get; set; }

    public virtual Group? Group { get; set; }
}
