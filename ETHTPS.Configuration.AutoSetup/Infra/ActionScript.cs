using ETHTPS.Configuration.AutoSetup.Infra.Exceptions.Scripts;

namespace ETHTPS.Configuration.AutoSetup.Infra;

internal class ActionScript : SetupScript
{
    #region Variables
    private readonly Action _script;
    private readonly Action? _pre;
    private readonly Action? _post;
    private readonly Action? _clean;
    private string? _details;
    #endregion

    #region Constructors

    public ActionScript(Action run, Action? pre, Action? post, Action? clean, string? details = null) : this(run, pre, post, details)
    {
        _clean = clean;
    }

    public ActionScript(Action run, Action? pre, Action? post, string? details = null) : this(run, pre, details)
    {
        _post = post;
    }

    public ActionScript(Action run, Action? pre, string? details = null) : this(run, details)
    {
        _pre = pre;
    }

    public ActionScript(Action run, string? details = null)
    {
        _script = run;
        _details = details;
    }

    #endregion

    public override void Pre()
    {
        try
        {
            _pre?.Invoke();
        }
        catch (Exception e)
        {
            throw new PreException(e);
        }
    }

    public override void Run()
    {
        try
        {
            _script.Invoke();
            if (!string.IsNullOrWhiteSpace(_details)) Logger.Ok(_details);
        }
        catch (Exception e)
        {
            string? m = string.IsNullOrWhiteSpace(_details) ? null : $"{_details}, {e.Message}";
            if (!string.IsNullOrWhiteSpace(m))
            {
                Logger.Error(m);
                throw new SetupScriptException(m, e);
            }
            else
            {
                throw new SetupScriptException(e);
            }
        }
    }

    public override void Post()
    {
        try
        {
            _post?.Invoke();
        }
        catch (Exception e)
        {
            throw new PostException(e);
        }
    }

    public override void Clean()
    {
        _clean?.Invoke();
    }
}