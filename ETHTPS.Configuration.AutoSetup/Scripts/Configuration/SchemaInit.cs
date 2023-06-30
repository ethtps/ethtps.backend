using System.ComponentModel.DataAnnotations.Schema;

using ETHTPS.Configuration.AutoSetup.Infra;

using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration;

/// <summary>
/// A class for checking whether a database context has the required schemas.
/// </summary>
/// <typeparam name="TContext"></typeparam>
internal sealed class SchemaInit<TContext> : SetupScript
where TContext : DbContext
{
    private const string _utilsProjectName = "ethtps.utils";
    private readonly TContext _context;
    private readonly string[] _schemas;

    public SchemaInit(TContext context, params string[] schemas)
    {
        _context = context;
        _schemas = schemas;
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