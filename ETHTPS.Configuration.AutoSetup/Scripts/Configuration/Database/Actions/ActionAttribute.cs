namespace ETHTPS.Configuration.AutoSetup.Scripts.Configuration.Database.Actions;

/// <summary>
/// Marks a class as a member containing one or multiple actions to be performed on a table.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal class ActionAttribute : Attribute
{
    public ActionAttribute(TableAction action)
    {
        Action = action;
    }

    /// <summary>
    /// Gets the to-be-performed action.
    /// </summary>
    public TableAction Action { get; private set; }
}