using System;

namespace ETHTPS.Data.Core.Models.Queries.Data.Requests.Extensions;

public static class RequestExtensions
{
    /// <summary>
    /// Validates the request model.
    /// </summary>
    /// <param name="source"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void Validate(this L2AdditionRequestModel source)
    {
        if (string.IsNullOrWhiteSpace(source.NetworkName))
        {
            throw new ArgumentException("NetworkName is required and can't be empty");
        }

        if (string.IsNullOrWhiteSpace(source.Type))
        {
            throw new ArgumentException("Type is required and can't be empty");
        }

        if (string.IsNullOrWhiteSpace(source.ProjectWebsite) || !Uri.TryCreate(source.ProjectWebsite, UriKind.Absolute, out var uriResult) || !(uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
        {
            throw new ArgumentException("ProjectWebsite is required and should be a valid HTTP or HTTPS URL");
        }

        if (!string.IsNullOrWhiteSpace(source.BlockExplorerURL) && (!Uri.TryCreate(source.BlockExplorerURL, UriKind.Absolute, out uriResult) || !(uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)))
        {
            throw new ArgumentException("BlockExplorerURL should be a valid HTTP or HTTPS URL");
        }
    }
}