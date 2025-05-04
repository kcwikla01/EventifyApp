using Eventify.Database.Models.Dto;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EventReviewController : Controller
    {
        private readonly IEventReviewApplicationService _eventReviewApplicationService;

        public EventReviewController(IEventReviewApplicationService eventReviewApplicationService)
        {
            this._eventReviewApplicationService = eventReviewApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> AddEventReview(EventReviewDto eventReviewDto)
        {
            return await _eventReviewApplicationService.AddEventReview(eventReviewDto);
        }
    }
}
