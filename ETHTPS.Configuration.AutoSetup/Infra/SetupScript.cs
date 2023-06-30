namespace ETHTPS.Configuration.AutoSetup.Infra;

/// <summary>
/// Represents an auto setup script.
/// </summary>
internal abstract class SetupScript
{
    private readonly IList<SetupScript> _children;
    private bool _ranChildren = false;

    internal void AddChild(SetupScript child)
    {
        _children.Add(child);
    }

    internal void AddChild<T>()
    where T : SetupScript, new()
    {
        _children.Add(new T());
    }

    protected SetupScript()
    {
        _children = new List<SetupScript>();
    }

    protected SetupScript(params SetupScript[] children)
    {
        _children = children;
    }

    /// <summary>
    /// Represents a script that runs before the main script. It is used to set up the environment for the main script.
    /// </summary>
    public virtual void Pre() { }

    /// <summary>
    /// Represents the main setup script. Everything that needs to be done to set up the environment should be done here. Calling the base method will run all children scripts.
    /// </summary>
    public virtual void Run()
    {
        RunChildren();
        _ranChildren = true;
    }

    private void RunChildren()
    {
        Logger.LeftPadding.Increment();
        foreach (var child in _children)
        {
            child.Pre();
            child.Run();
            child.Post();
            child.Clean();
        }
        Logger.LeftPadding.Decrement();
    }

    /// <summary>
    /// Represents a script that runs after the main script. It is used to run validations of the main script.
    /// </summary>
    public virtual void Post()
    {
        if (!_ranChildren && _children.Count > 0)
        {
            Logger.Warn("Children scripts not ran");
        }
    }

    /// <summary>
    /// Represents a script that cleans up the environment after setup.
    /// </summary>
    public virtual void Clean() { }
}