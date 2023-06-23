namespace ETHTPS.Data.Core.Models.Queries.Data.Requests;

/// <summary>
/// Represents a user's request to include a new L2.
/// </summary>
public class L2AdditionRequestModel
{
    public required string NetworkName { get; set; }
    public required string Type { get; set; }
    public required string ProjectWebsite { get; set; }
    public string? ShortDescription { get; set; }
    public string? BlockExplorerURL { get; set; }
}