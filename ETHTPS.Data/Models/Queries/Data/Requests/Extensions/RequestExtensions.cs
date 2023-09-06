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

        if (!ValidateURL(source.ProjectWebsite))
        {
            throw new ArgumentException("ProjectWebsite is required and should be a valid HTTP(s) URL");
        }
    }

    private static bool ValidateURL(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;
        if (!url.StartsWith("http://") && !url.StartsWith("https://")) url = $"https://{url}";
        return Uri.IsWellFormedUriString("https://www.google.com", UriKind.Absolute);
    }
}