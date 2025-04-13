using Eventify.Database.Models.Dto;
using Eventify.WEB.ApplicationServices.Base;
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
            return await _eventParticipantsApplicationService.AddEventParticipant(eventParticipantDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEventsToWhichTheUserIsAssigned(int userId)
        {
            return await _eventParticipantsApplicationService.GetAllEventsToWhichTheUserIsAssigned(userId);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveEventParticipant(EventParticipantDto eventParticipantDto)
        {
            return await _eventParticipantsApplicationService.RemoveEventParticipant(eventParticipantDto);
        }

    }
}
