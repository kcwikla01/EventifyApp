using Eventify.Database.Models.Dto;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EventController : Controller
    {
        private readonly IEventApplicationService _eventApplicationService;
       public EventController(IEventApplicationService eventApplicationService)
        {
            _eventApplicationService = eventApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventDto eventDto)
        {
            var userId = GetUserIdFromHeaders();
            if (userId == null)
            {
                return Unauthorized("User ID is required or invalid.");
            }
            return await _eventApplicationService.CreateEvent(eventDto, userId.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetEventById(int id)
        {
            return await _eventApplicationService.GetEventById(id);
        }


        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            return await _eventApplicationService.GetEvents();
        }


        [HttpGet]
        public async Task<IActionResult> GetEventsByOwnerId(int ownerId)
        {
            var userId = GetUserIdFromHeaders();
            if (userId == null)
            {
                return Unauthorized("User ID is required or invalid.");
            }
            return await _eventApplicationService.GetEventsByOwnerId(ownerId, userId.Value);
        }




        [HttpDelete]
        public async Task<IActionResult> RemoveEventById(int id)
        {
            var userId = GetUserIdFromHeaders();
            if (userId == null)
            {
                return BadRequest("User ID is required or invalid.");
            }
            return await _eventApplicationService.RemoveEventById(id, userId.Value);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateEventById(EventDto eventDto)
        {
            var userId = GetUserIdFromHeaders();
            if (userId == null)
            {
                return Unauthorized("User ID is required or invalid.");
            }
            return await _eventApplicationService.UpdateEventById(eventDto, userId.Value);
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
