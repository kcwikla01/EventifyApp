using Eventify.Database.Models.Dto;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices
{
    public class EventShedulesApplicationService : IEventShedulesApplicationService
    {
        public Task<IActionResult> RemoveActivity(int activityId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetAllActivitiesForEvent(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetActivityInfo(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> AddEventShedules(EventActivityDto eventActivityDto)
        {
            throw new NotImplementedException();
        }
    }
}
