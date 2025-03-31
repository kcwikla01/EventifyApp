using Eventify.Database.Models.Dto;
using Eventify.WEB.ApplicationServices.Base;
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
            return await _eventApplicationService.CreateEvent(eventDto);
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
            return await _eventApplicationService.GetEventsByOwnerId(ownerId);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveEventById(int id)
        {
            return await _eventApplicationService.RemoveEventById(id);
        }
    }
}
