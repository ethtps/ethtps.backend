﻿using System.Collections.Generic;

using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.API.Core.Integrations.MSSQL.Controllers.CRUD;
using ETHTPS.Data.Core.Models.Markdown;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/markdown-pages/[action]")]
    [ApiController]
    public sealed class MarkdownPagesController : CRUDServiceControllerBase<MarkdownPage>
    {
        private readonly IMarkdownService _markdownService;
        public MarkdownPagesController(IMarkdownService serviceImplementation) : base((ICRUDService<MarkdownPage>)serviceImplementation) => _markdownService = serviceImplementation;

        [HttpGet]
        public IEnumerable<IMarkdownPage> GetMarkdownPagesFor(string providerName) => _markdownService.GetMarkdownPagesFor(providerName);
    }
}
