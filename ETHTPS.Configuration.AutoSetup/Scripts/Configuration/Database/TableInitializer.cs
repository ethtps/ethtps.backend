using System.ComponentModel.DataAnnotations.Schema;

using ETHTPS.Configuration.AutoSetup.Infra;
using ETHTPS.Configuration.AutoSetup.Scripts.Configuration.Database.Actions;
using ETHTPS.Data.Core.Extensions;

using Microsoft.EntityFrameworkCore;

using static ETHTPS.Data.Core.Attributes.AttributeExtensions;
namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration.Database;

/// <summary>
/// A class for checking whether a database context has all the required tables in the required form (
/// .definitions + constraints + data).
/// </summary>
/// <typeparam name="TContext"></typeparam>
[Action(TableAction.Initialize)]
internal sealed class TableInitializer<TContext> : SetupScript
    where TContext : DbContext
{
    #region Variables + types

    private const string _utilsProjectName = "ethtps.utils";
    private const string _creationScriptFileName = "create.sql";
    private const string _populationScriptFileName = "populate.sql";
    private const string _constraintScriptFileName = "constrain.sql";
    private readonly TContext _context;
    private readonly string[] _schemas;

    [NotMapped]
    public class TableInfo
    {
        public required string SchemaName { get; set; }
        public required string TableName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public override string ToString() => $"[{SchemaName}].[{TableName}]";
    }

    #endregion

    #region  Constructors

    public TableInitializer(TContext context, params string[] schemas)
    {
        _context = context;
        _schemas = schemas;
    }

    #endregion

    #region SetupScript

    public override void Run()
    {
        var rootPath = Utils.TryGetSolutionDirectoryInfo()?.Parent?.FullName;
        Assert.Directory.Exists(rootPath);
        rootPath ??= string.Empty;
        var utilsPath = Path.Combine(rootPath, _utilsProjectName);
        Assert.Directory.Exists(utilsPath);
        var sqlDirectory = Path.Combine(utilsPath, "sql");
        Assert.Directory.Exists(sqlDirectory);
        // Validate all tables in all schemas
        foreach (var schema in _schemas)
        {
            ActOn(schema, TableAction.Validate);
        }
        base.Run();
    }


    #endregion

    #region Action creation methods
    private void ActOn(string schema, TableAction action)
    {
        var actionDescription = action.GetAttribute<HasSqlScriptAttribute>().Description ?? action.ToString();
        Logger.Info($"[{schema}]: {actionDescription}");
        Logger.Debug($"Acting on schema");
        var tables = GetTablesForSchema(schema);
        Logger.Debug($"Found the following tables: [{string.Join(',', tables)}] (no filters applied)");
        foreach (var table in tables)
        {
            Logger.Debug($"[{schema}]: {action}({table})");
            var task = CreateAction(table, action);
            if (task != null)
            {
                AddChild(task);
            }
        }
    }

    private TableAction<TContext>? CreateAction(TableInfo table, TableAction action) =>
        CreateAction(table.SchemaName, table.TableName, action);
    private TableAction<TContext>? CreateAction(string schema, string tableName, TableAction action)
    {
        var fullName = $"[{schema}].[{tableName}]";
        var scriptFile =
            action.GetAttribute<HasSqlScriptAttribute>();
        if (scriptFile == null)
        {
            Logger.Warn($"No script file name defined for action \"{action}\". Member needs to be marked as [HasSqlScript].");
            return null;
        }

        var path = Path.Combine(scriptFile.BaseDirectory, schema, tableName, scriptFile.ScriptFileName);
        if (!Assert.Controlled(() => Assert.File.Exists(path), $"{action}({fullName})"))
        {
            Logger.Warn($"{fullName}: file {scriptFile.FullPath} not found");
            return null;
        }
        return new TableAction<TContext>(schema, tableName, _context, action);
    }

    private IEnumerable<SetupScript> CreateActions(IEnumerable<KeyValuePair<string, string>> schemasAndTables, TableAction action) =>
        schemasAndTables.Select(st => CreateAction(st.Key, st.Value, action)).WhereNotNull()!;


    #endregion

    #region Utils

    private List<TableInfo> GetTablesForSchema(string schema)
    {
        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = $@"
        SELECT schema_name(t.schema_id) as schema_name,
               t.name as table_name,
               t.create_date,
               t.modify_date
        FROM sys.tables t
        WHERE schema_name(t.schema_id) = '{schema}' 
        ORDER BY table_name";

        _context.Database.OpenConnection();

        var tableInfos = new List<TableInfo>();
        using var result = command.ExecuteReader();

        while (result.Read())
        {
            var tableInfo = new TableInfo
            {
                SchemaName = result.GetString(0),
                TableName = result.GetString(1),
                CreateDate = result.GetDateTime(2),
                ModifyDate = result.GetDateTime(3)
            };

            tableInfos.Add(tableInfo);
        }

        return tableInfos;
    }

    #endregion
}