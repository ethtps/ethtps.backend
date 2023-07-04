namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration.Database;

/// <summary>
/// Represents an operation that can be performed on a table.
/// </summary>
internal enum TableAction
{
    Initialize,
    /// <summary>
    /// Create a table.
    /// </summary>
    [HasSqlScript("init.sql")]
    Create,
    /// <summary>
    /// Delete a table.
    /// </summary>
    [HasSqlScript("delete.sql")]
    Delete,
    /// <summary>
    /// Validate the definition of a table. If the definition is not valid, try to fix it by running remove constraints, delete, create, populate and lastly - constrain.
    /// </summary>
    [HasSqlScript("validate.sql")]
    ValidateAndFix,
    /// <summary>
    /// Populate a table and its relatives with data.
    /// </summary>
    [HasSqlScript("populate.sql")]
    Populate,
    /// <summary>
    /// Add constraints to a table.
    /// </summary>
    [HasSqlScript("constrain.sql", "Add constraints")]
    Constrain,
    /// <summary>
    /// Remove constraints from a table.
    /// </summary>
    [HasSqlScript("deconstrain.sql", "Remove all constraints")]
    RemoveConstraints
}