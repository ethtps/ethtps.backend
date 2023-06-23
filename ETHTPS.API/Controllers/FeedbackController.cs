using System;

using ETHTPS.API.Core.Controllers;
using ETHTPS.Data.Core.Models;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Core.Models.Queries.Data.Requests.Extensions;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Integrations.MSSQL.Extensions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

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
        public IActionResult RequestNewL2([FromBody] L2AdditionRequestModel model)
        {
            try
            {
                model.Validate();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            lock (_context.LockObj)
            {
                _context.UserFeedback.Add(new()
                {
                    Type = _context.GetFeedbackTypeID("L2AdditionRequest"),
                    Title = $"Request to include {model.NetworkName}",
                    Details = model.ShortDescription,
                    ExtraData = JsonConvert.SerializeObject(model)
                });
                _context.SaveChanges();
            }

            return StatusCode(201);
        }

        [HttpPost]
        public IActionResult ReportIssue([FromBody] IssueModel issue)
        {
            if (issue?.Text?.Length > 0)
            {
                lock (_context.LockObj)
                {
                    _context.UserFeedback.Add(new()
                    {
                        Title = "Issue report",
                        Details = issue.Text,
                        Type = _context.GetFeedbackTypeID("InvalidData")
                    });
                    _context.SaveChanges();
                    return StatusCode(201);
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
