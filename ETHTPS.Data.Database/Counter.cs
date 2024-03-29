﻿namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Counter
{
    public required string Key { get; set; }

    public int Value { get; set; }

    public DateTime? ExpireAt { get; set; }

    public long Id { get; set; }
}
