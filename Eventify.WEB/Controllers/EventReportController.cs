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
            return await _eventReportApplicationService.GenerateReport(eventId);
        }
    }
}
