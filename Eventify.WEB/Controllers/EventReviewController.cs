using Eventify.Database.Models.Dto;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Authorization;
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
            var userId = GetUserIdFromHeaders();
            if (userId == null)
            {
                return Unauthorized("User ID is required or invalid.");
            }
            return await _eventReviewApplicationService.AddEventReview(eventReviewDto, userId.Value);
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
