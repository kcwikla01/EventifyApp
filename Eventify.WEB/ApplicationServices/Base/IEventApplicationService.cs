using Eventify.Database.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices.Base
{
    public interface IEventApplicationService
    {
        Task<IActionResult> CreateEvent(EventDto eventDto, int userId);
        Task<IActionResult> GetEventById(int id);
        Task<IActionResult> GetEvents();
        Task<IActionResult> GetEventsByOwnerId(int ownerId, int userId);
        Task<IActionResult> RemoveEventById(int id, int userId);
        Task<IActionResult> UpdateEventById(EventDto eventDto, int userId);
    }
}
