using Eventify.Database.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices.Base
{
    public interface IEventParticipantsApplicationService
    {
        Task<IActionResult> AddEventParticipant(EventParticipantDto eventParticipantDto);
        Task<IActionResult> GetAllEventsToWhichTheUserIsAssigned(int userId);
        Task<IActionResult> RemoveEventParticipant(EventParticipantDto eventParticipantDto);
    }
}
