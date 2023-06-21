using ETHTPS.API.Core.Controllers;
using ETHTPS.Data.Core.Models;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETHTPS.API.Controllers
{
    [Route("/api/v3/Feedback/[action]")]
    [ApiController]
    [Authorize]
    public sealed class FeedbackController : APIControllerBase
    {
        private readonly EthtpsContext _context;

        public FeedbackController(EthtpsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult ReportIssue([FromBody] IssueModel issue)
        {
            if (issue?.Text?.Length > 0)
            {
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
