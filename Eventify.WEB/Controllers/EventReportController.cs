using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EventReportController : Controller
    {
        private readonly IEventReportApplicationService _eventReportApplicationService;

        public EventReportController(IEventReportApplicationService eventReportApplicationService)
        {
            _eventReportApplicationService = eventReportApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport(int eventId)
        {
            var userId = GetUserIdFromHeaders();
            if (userId == null)
            {
                return Unauthorized("User ID is required or invalid.");
            }
            return await _eventReportApplicationService.GenerateReport(eventId, userId.Value);
        }

        private int? GetUserIdFromHeaders()
        {
            if (HttpContext.Request.Headers.TryGetValue("user-id", out var userIdValue) && int.TryParse(userIdValue, out var userId))
            {
                return userId;
            }

            return null;
        }
    }
}
