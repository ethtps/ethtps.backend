using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.API.Core.Attributes;

using Microsoft.AspNetCore.Http;

namespace ETHTPS.API.Core.Middlewares
{
    /// <summary>
    /// Represents a middleware that caches responses for a specified TTL using Redis. Must be placed AFTER UseRouting(), otherwise the endpoint will be null and nothing will be cached.
    /// </summary>
    public class RedisCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRedisCacheService _cacheService;

        public RedisCacheMiddleware(RequestDelegate next, IRedisCacheService cacheService)
        {
            _next = next;
            _cacheService = cacheService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var attribute = endpoint?.Metadata.GetMetadata<TTLAttribute>();
            if (attribute == null)
            {
                await _next(context);
                return;
            }
            var cacheKey = GetCacheKey(context);
            var response = await _cacheService.GetDataAsync<string>(cacheKey);
            if (response != null)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(response);
                return;
            }
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context);
                if (context.Response.StatusCode == StatusCodes.Status200OK)
                {
                    var responseBodyString = await GetResponseBodyString(context.Response);
                    await _cacheService.SetDataAsync(cacheKey, responseBodyString, attribute.TTL);
                }
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private static string GetCacheKey(HttpContext context)
        {
            var path = context.Request.Path.ToString();
            var queryString = context.Request.QueryString.ToString();
            return $"request_cache:{path}:{queryString}";
        }

        private static async Task<string> GetResponseBodyString(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyString = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return responseBodyString;
        }
    }

}