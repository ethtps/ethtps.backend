using ETHTPS.Configuration.AutoSetup.Infra.Exceptions.Scripts;

namespace ETHTPS.Configuration.AutoSetup.Infra;

internal class ActionScript : SetupScript
{
    private readonly Action _script;
    private readonly Action? _pre;
    private readonly Action? _post;
    private readonly Action? _clean;

    public ActionScript(Action run, Action? pre, Action? post, Action? clean)
    {
        _script = run;
        _pre = pre;
        _post = post;
        _clean = clean;
    }

    public ActionScript(Action run, Action? pre, Action? post)
    {
        _script = run;
        _pre = pre;
        _post = post;
    }

    public ActionScript(Action run, Action? pre)
    {
        _script = run;
        _pre = pre;
    }

    public ActionScript(Action run)
    {
        _script = run;
    }

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
        }
        catch (Exception e)
        {
            throw new SetupScriptException(e);
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