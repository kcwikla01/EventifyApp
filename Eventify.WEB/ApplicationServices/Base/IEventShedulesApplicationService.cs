using Eventify.Database.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices.Base
{
    public interface IEventShedulesApplicationService
    {
        Task<IActionResult> RemoveActivity(int activityId, int userId);
        Task<IActionResult> GetAllActivitiesForEvent(int eventId);
        Task<IActionResult> GetActivityInfo(int activityId);
        Task<IActionResult> AddEventShedules(EventActivityDto eventActivityDto, int userId);
    }
}
