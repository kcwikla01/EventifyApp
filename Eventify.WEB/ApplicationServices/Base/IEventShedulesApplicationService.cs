using Eventify.Database.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices.Base
{
    public interface IEventShedulesApplicationService
    {
        Task<IActionResult> RemoveActivity(int activityId);
        Task<IActionResult> GetAllActivitiesForEvent(int eventId);
        Task<IActionResult> GetActivityInfo(int id);
        Task<IActionResult> AddEventShedules(EventActivityDto eventActivityDto);
    }
}
