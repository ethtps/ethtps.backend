﻿using System.Security.Claims;
using System.Text.Encodings.Web;

using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Integrations.MSSQL.Extensions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace ETHTPS.API.Security.Core.Authentication
{
    public sealed class APIKeyAuthenticationSchemeHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly EthtpsContext _context;
        private readonly ILogger _logger;
        public APIKeyAuthenticationSchemeHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
#pragma warning disable CS0618 // Suggests something that can't be used ??
            ISystemClock clock,
            EthtpsContext context) : base(options, logger, encoder, clock)
#pragma warning restore CS0618 
        {
            _context = context;
            _logger = logger.CreateLogger<APIKeyAuthenticationSchemeHandler>();
        }

        private bool ValidateAPIKey(StringValues apikey)
        {
            if (string.IsNullOrWhiteSpace(apikey)) return false;
            return _context.ValidateAPIKey(apikey);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKey = Context.ExtractAPIKey();
            if (!ValidateAPIKey(apiKey))
            {
                _logger.LogWarning("API call with invalid key [{0}]", Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString());
                return Task.FromResult(AuthenticateResult.Fail("No X-API-KEY header specified or API key is invalid"));
            }

            if (!_context.ValidateNumberOfCalls(apiKey))
            {
                _logger.LogWarning("Limit rate exceeded [{0}]", Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString());
                return Task.FromResult(AuthenticateResult.Fail("Limit reached for today"));
            }

            _context.IncrementNumberOfCalls(apiKey);
            Claim[] claims = new[] { new Claim(ClaimTypes.Name, "VALID USER") };
            ClaimsIdentity identity = new(claims, Scheme.Name);
            ClaimsPrincipal principal = new(identity);
            AuthenticationTicket ticket = new(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
