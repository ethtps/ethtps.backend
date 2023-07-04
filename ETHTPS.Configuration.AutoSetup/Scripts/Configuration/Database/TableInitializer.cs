using System.ComponentModel.DataAnnotations.Schema;

using ETHTPS.Configuration.AutoSetup.Infra;
using ETHTPS.Configuration.AutoSetup.Scripts.Configuration.Database.Actions;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Core.Extensions.StringExtensions;

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
    private readonly TContext _context;
    private readonly string[] _tablesToCreate;
    private readonly string _existingSchema;
    private readonly string _projectDirectory;

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

    /// <summary>
    /// Initializes a new instance of the <see cref="TableInitializer{TContext}"/> class.
    /// </summary>
    /// <param name="context">An instance of the context to work on</param>
    /// <param name="existingSchema"><para>An assumed-to-exist schema.</para>
    /// <para>Providing an invalid value will lead to unexpected results. </para>
    /// <para>It is assumed this class is used only *after* having ran <seealso cref="SchemaInitializer{TContext}"/>.</para>
    /// </param>
    /// <param name="tablesToCreate">A list of tables to be created. Only *new* tables should be specified as the initializer doesn't check for this.</param>
    public TableInitializer(TContext context, string existingSchema, params string[] tablesToCreate)
    {
        _context = context;
        _tablesToCreate = tablesToCreate;
        _existingSchema = existingSchema;
        _projectDirectory = Utils.TryGetSolutionDirectoryInfo()?.Parent?.FullName ?? string.Empty;
    }

    /// <summary>
    /// <para> Initializes a new instance of the <see cref="TableInitializer{TContext}"/> class.</para>
    /// <para> It is assumed that the directory ../[ProjectDir]/ethtps.utils/sql/[existingSchema]) exists and contains the table definitions.</para>
    /// </summary>
    /// <param name="context">An instance of the context to work on</param>
    /// <param name="existingSchema"><para>An assumed-to-exist schema.</para>
    /// <para>Providing an invalid value will lead to unexpected results. </para>
    /// <para>It is assumed this class is used only *after* having ran <seealso cref="SchemaInitializer{TContext}"/>.</para>
    /// </param>
    public TableInitializer(TContext context, string existingSchema)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _existingSchema = existingSchema ?? throw new ArgumentNullException(nameof(existingSchema));
        var path = Path.Combine(Utils.SqlDirectoryPath, existingSchema.RemoveAllNonAlphaNumericCharacters().ToLower());
        Assert.Directory.Exists(path);
        _tablesToCreate = Assert.DoesNotThrow(() => Directory.GetDirectories(path).Select(x => (new DirectoryInfo(x)).Name).Where(x => x != "procedures").ToArray(), $"List required table definitions in [{existingSchema}]");
        Logger.Info($"Found {_tablesToCreate.Length} table{(_tablesToCreate.Length != 1 ? "s" : string.Empty)} to validate and initialize");
        _projectDirectory = Utils.TryGetSolutionDirectoryInfo()?.Parent?.FullName ?? string.Empty;
    }

    #endregion

    #region SetupScript

    public override void Run()
    {
        var kvps = _tablesToCreate.Select(t => new KeyValuePair<string, string>(_existingSchema, t)).ToList();
        if (kvps.Any())
        {
            AddChildren(CreateActions(kvps, TableAction.Create, null, false)); // The app will crash like mad without tables
            AddChildren(CreateActions(kvps, TableAction.Populate));
            AddChildren(CreateActions(kvps, TableAction.Constrain));
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

    private TableAction<TContext>? CreateAction(TableInfo table, TableAction action, bool dontThrow = true) =>
        CreateAction(table.SchemaName, table.TableName, action, dontThrow);

    private TableAction<TContext>? CreateAction(string schema, string tableName, TableAction action, bool dontThrow = true)
    {
        var fullName = $"[{schema}].[{tableName}]";
        var scriptFile =
            action.GetAttribute<HasSqlScriptAttribute>();
        if (scriptFile == null)
        {
            Logger.Warn($"No script file name defined for action \"{action}\". Member needs to be marked as [HasSqlScript].");
            return null;
        }
        var details = $"{action}({fullName})";
        return new TableAction<TContext>(tableName, schema, _context, action)
        {
            Details = details,
            DontThrow = dontThrow
        };
    }

    private IEnumerable<SetupScript> CreateActions(IEnumerable<KeyValuePair<string, string>> schemasAndTables,
        TableAction action, in Func<KeyValuePair<string, string>, bool>? filter = null, bool dontThrow = true) =>
        schemasAndTables.Where(
            filter ?? (_ => true))
            .Select(st =>
                CreateAction(st.Key, st.Value, action, dontThrow))
            .WhereNotNull()!;


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