using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventify.Database.Models;
using Eventify.Database.Models.Dto;

namespace Eventify.UoW.Base
{
    public interface IManageEventShedulesUoW
    {
        Task<EventActivityDto> AddEventActivity(EventActivityDto eventActivityDto);
        Task<bool> RemoveEventActivity(EventSchedule eventSchedule);
        Task<EventSchedule?> GetEventActivity(int activityId);
        Task<List<EventActivityDto>> GetEventsActivities(int eventId);
        Task<bool> CheckIfHasConflictingActivity(EventActivityDto eventActivityDto);
    }
}
