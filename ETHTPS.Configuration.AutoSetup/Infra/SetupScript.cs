namespace ETHTPS.Configuration.AutoSetup.Infra;

/// <summary>
/// Represents an auto setup script.
/// </summary>
internal abstract class SetupScript
{
    private IEnumerable<SetupScript>? _children;
    public Guid GUID { get; } = Guid.NewGuid();

    protected SetupScript()
    {

    }

    protected SetupScript(params SetupScript[] children)
    {
        _children = children;
        foreach (var child in children)
        {
            child.Pre();
            child.Run();
            child.Post();
            child.Clean();
        }
    }

    /// <summary>
    /// Represents a script that runs before the main script. It is used to set up the environment for the main script.
    /// </summary>
    public abstract void Pre();

    /// <summary>
    /// Represents the main setup script. Everything that needs to be done to set up the environment should be done here.
    /// </summary>
    public abstract void Run();

    /// <summary>
    /// Represents a script that runs after the main script. It is used to run validations of the main script.
    /// </summary>
    public abstract void Post();

    /// <summary>
    /// Represents a script that cleans up the environment after setup.
    /// </summary>
    public abstract void Clean();
}