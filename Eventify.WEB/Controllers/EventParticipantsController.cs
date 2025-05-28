using Eventify.Database.Models.Dto;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EventParticipantsController : Controller
    {
        private readonly IEventParticipantsApplicationService _eventParticipantsApplicationService;

        public EventParticipantsController(IEventParticipantsApplicationService eventParticipantsApplicationService)
        {
            _eventParticipantsApplicationService = eventParticipantsApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> AddEventParticipant(EventParticipantDto eventParticipantDto)
        {
            var userId = GetUserIdFromHeaders();
            if (userId == null)
            {
                return Unauthorized("User ID is required or invalid.");
            }
            return await _eventParticipantsApplicationService.AddEventParticipant(eventParticipantDto, userId.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEventsToWhichTheUserIsAssigned(int userId)
        {
            return await _eventParticipantsApplicationService.GetAllEventsToWhichTheUserIsAssigned(userId);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveEventParticipant(EventParticipantDto eventParticipantDto)
        {
            var userId = GetUserIdFromHeaders();
            if (userId == null)
            {
                return Unauthorized("User ID is required or invalid.");
            }
            return await _eventParticipantsApplicationService.RemoveEventParticipant(eventParticipantDto,userId.Value);
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
