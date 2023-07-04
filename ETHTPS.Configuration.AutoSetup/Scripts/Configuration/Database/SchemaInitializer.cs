using System.ComponentModel.DataAnnotations.Schema;

using ETHTPS.Configuration.AutoSetup.Infra;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration.Database;

/// <summary>
/// A class for checking whether a database context has the required schemas and creating them in case they are missing.
/// </summary>
/// <typeparam name="TContext"></typeparam>
internal sealed class SchemaInitializer<TContext> : SetupScript
    where TContext : DbContext
{
    private const string _utilsProjectName = "ethtps.utils";
    private readonly TContext _context;
    private readonly string[] _schemas;

    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaInitializer{TContext}"/> class.
    /// </summary>
    /// <param name="context">An instance of the context to work on</param>
    /// <param name="schemas"></param>
    public SchemaInitializer(TContext context, params string[] schemas)
    {
        _context = context;
        _schemas = schemas;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaInitializer{TContext}"/> class, attempts to load the schemas from the default file (../[ProjectDir]/ethtps.utils/sql/schemas.json) while assuming the context needs to have all tables belonging to the defined schemas present.
    /// </summary>
    /// <param name="context">An instance of the context to work on</param>
    public SchemaInitializer(TContext context)
    {
        _context = context;
        var rootPath = Utils.TryGetSolutionDirectoryInfo()?.Parent?.FullName ?? string.Empty;
        Assert.Directory.Exists(rootPath);
        var utilsPath = Path.Combine(rootPath, _utilsProjectName);
        Assert.Directory.Exists(utilsPath);
        var sqlDirectory = Path.Combine(utilsPath, "sql");
        Assert.Directory.Exists(sqlDirectory);
        var schemaFilePath = Path.Combine(sqlDirectory, "schemas.json");
        Assert.File.Exists(schemaFilePath);
        _schemas = Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<string[]>(File.ReadAllText(schemaFilePath)), "Get schemas from default file")!;
    }

    [NotMapped]
    public class Schema
    {
        public required string SchemaName { get; set; }
    }

    public override void Run()
    {
        var schemas = GetDatabaseSchemas();
        var isOk = _schemas.All(s => schemas.Contains(s));
        if (isOk)
        {
            Logger.Ok($"Database has all required schemas [{string.Join(',', _schemas)}]");
            return;
        }
        var nonExistent = _schemas.Where(s => !schemas.Contains(s)).ToList();
        Logger.Warn($"Missing the following schemas: [{string.Join(',', nonExistent)}]");
        foreach (var schema in nonExistent)
        {
            Logger.Info($"Creating schema [{schema}]");
            _context.Database.ExecuteSqlRaw($"CREATE SCHEMA [{schema}]");
        }

        schemas = GetDatabaseSchemas();
        isOk = _schemas.All(s => schemas.Contains(s));
        Assert.That(isOk, $"Database has all required schemas [{string.Join(',', _schemas)}]");
    }

    private string[] GetDatabaseSchemas()
    {
        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = "SELECT s.name as schema_name FROM sys.schemas s INNER JOIN sys.sysusers u ON u.uid = s.principal_id ORDER BY s.name";

        _context.Database.OpenConnection();

        using var result = command.ExecuteReader();
        var schemas = new List<string>();

        while (result.Read())
        {
            schemas.Add(result.GetString(0));
        }

        return schemas.ToArray();
    }
}

/*
        var rootPath = Utils.TryGetSolutionDirectoryInfo()?.Parent?.FullName;
        Assert.Directory.Exists(rootPath);
        rootPath ??= string.Empty;
        var utilsPath = Path.Combine(rootPath, _utilsProjectName);
        Assert.Directory.Exists(utilsPath);
        var sqlDirectory = Path.Combine(utilsPath, "sql", "configuration");
        Assert.Directory.Exists(utilsPath);
 */