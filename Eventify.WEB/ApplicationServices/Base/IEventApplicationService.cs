using Eventify.Database.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices.Base
{
    public interface IEventApplicationService
    {
        Task<IActionResult> CreateEvent(EventDto eventDto);
        Task<IActionResult> GetEventById(int id);
        Task<IActionResult> GetEvents();
        Task<IActionResult> GetEventsByOwnerId(int ownerId);
        Task<IActionResult> RemoveEventById(int id);
        Task<IActionResult> UpdateEventById(EventDto eventDto);
    }
}
