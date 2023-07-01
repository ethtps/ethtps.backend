using ETHTPS.Configuration.AutoSetup.Infra;
using ETHTPS.Data.Core.Attributes;

using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration.Database.Actions
{
    /// <summary>
    /// A base class for quickly defining an action to be performed on a table.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    internal sealed class TableAction<TContext> : SetupScript
        where TContext : DbContext
    {
        #region Variables

        private string FullTableName => $"[{_schema}].[{_tableName}]";
        private readonly string _tableName;
        private readonly string _schema;
        private readonly string _scriptFileName;
        private readonly string _scriptDirectory;
        private readonly TContext _context;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TableAction{TContext}"/> class.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="schema"></param>
        /// <param name="scriptFileName"></param>
        /// <param name="context"></param>
        /// <param name="scriptDirectory"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TableAction(string tableName, string schema, TContext context, string scriptFileName, string? scriptDirectory = null)
        {
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
            _scriptFileName = scriptFileName ?? throw new ArgumentNullException(nameof(scriptFileName));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _scriptDirectory = scriptDirectory
                               ??
                               Path.Combine(Utils.TryGetSolutionDirectoryInfo()?.Parent?.FullName ?? string.Empty, "ETHTPS.Utils", "sql", _schema.ToLower(), _tableName.ToLower());
        }

        /// <summary>
        /// <inheritdoc cref="TableAction{TContext}(string, string, TContext, string, string?)"/>
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        /// <param name="attribute"></param>
        public TableAction(string tableName,
            string schema,
            TContext context,
            HasSqlScriptAttribute attribute) :
            this(tableName,
                schema,
                context,
                attribute.ScriptFileName,
                Path.Combine(attribute.BaseDirectory,
                    schema,
                    tableName,
                    attribute.ScriptFileName))
        {

        }

        /// <summary>
        /// <inheritdoc cref="TableAction{TContext}(string, string, TContext, string, string?)"/>
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        /// <param name="action"></param>
        public TableAction(string tableName, string schema, TContext context, TableAction action) : this(tableName, schema, context, action.GetAttribute<HasSqlScriptAttribute>())
        {

        }



        #endregion

        public override void Run()
        {
            var path = Path.Combine(_scriptDirectory, _scriptFileName);
            Assert.File.Exists(path);
            var script = File.ReadAllText(_scriptFileName);
            var result = _context.Database.ExecuteSqlRaw(script);
            Logger.Info($"Execution result: {result} row(s) affected");
        }
    }
}
