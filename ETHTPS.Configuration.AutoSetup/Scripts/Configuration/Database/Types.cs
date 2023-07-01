namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration.Database;

/// <summary>
/// Represents an operation that can be performed on a table.
/// </summary>
internal enum TableAction
{
    /// <summary>
    /// Initialize a table by performing the following actions in this exact order: Validate, continue if ok, Create, Populate, Constrain.
    /// </summary>
    [HasSqlScript("init.sql")]
    Initialize,
    /// <summary>
    /// Create a table.
    /// </summary>
    [HasSqlScript("create.sql")]
    Create,
    /// <summary>
    /// Delete a table.
    /// </summary>
    [HasSqlScript("delete.sql")]
    Delete,
    /// <summary>
    /// Validate the definition o a table.
    /// </summary>
    [HasSqlScript("validate.sql")]
    Validate,
    /// <summary>
    /// Populate a table and its relatives with data.
    /// </summary>
    [HasSqlScript("populate.sql")]
    Populate,
    /// <summary>
    /// Add constraints to a table.
    /// </summary>
    [HasSqlScript("constrain.sql")]
    Constrain,
    /// <summary>
    /// Remove constraints from a table.
    /// </summary>
    [HasSqlScript("deconstrain.sql")]
    RemoveConstraints
}