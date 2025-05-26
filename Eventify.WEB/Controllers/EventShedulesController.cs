using Eventify.Database.Models.Dto;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EventShedulesController : Controller
    {
        private readonly IEventShedulesApplicationService _eventShedulesApplicationService;

        public EventShedulesController(IEventShedulesApplicationService eventShedulesApplicationService)
        {
            _eventShedulesApplicationService = eventShedulesApplicationService;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public async Task<IActionResult> AddEventActivity(EventActivityDto eventActivityDto)
        {
            return await _eventShedulesApplicationService.AddEventShedules(eventActivityDto);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete]
        public async Task<IActionResult> RemoveEventActivity(int activityId)
        {
            return await _eventShedulesApplicationService.RemoveActivity(activityId);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetAllActivitiesForEvent(int eventId)
        {
            return await _eventShedulesApplicationService.GetAllActivitiesForEvent(eventId);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetEventActivityInfo(int id)
        {
            return await _eventShedulesApplicationService.GetActivityInfo(id);
        }
    }
}
