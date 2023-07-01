namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration.Database;

/// <summary>
/// Marks an enum field as having a sql script associated with it.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
internal sealed class HasSqlScriptAttribute : Attribute
{
    public string ScriptFileName { get; private set; }
    public string BaseDirectory { get; private set; }
    public string FullPath => Path.Combine(BaseDirectory, ScriptFileName);
    public string? Description { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HasSqlScriptAttribute"/> class.
    /// </summary>
    /// <param name="scriptFileName">The name of the sql script file</param>
    /// <param name="scriptBaseDirectory">The directory the sql script file resides in. If null, it defaults to ../[ProjectDir]/ETHTPS.Utils/scripts/sql/</param>
    public HasSqlScriptAttribute(string scriptFileName, string? description = null, string? scriptBaseDirectory = null)
    {
        ScriptFileName = scriptFileName ?? throw new ArgumentNullException(nameof(scriptFileName));
        BaseDirectory = scriptBaseDirectory ?? Path.Combine(Utils.TryGetSolutionDirectoryInfo()?.Parent?.FullName ?? string.Empty, "ETHTPS.Utils", "sql");
        Description = description;
    }
}