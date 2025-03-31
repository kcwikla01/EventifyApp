using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventify.Database.Models;
using Eventify.Database.Models.Dto;

namespace Eventify.UoW.Base
{
    public interface IManageEventsUoW
    {
        Task<Event> CreateEvent(EventDto eventDto);
        Task<Event?> GetEventById(int id);
        Task<List<Event>> GetEvents();
        Task<List<Event>> GetEventsByOwnerId(int ownerId);
        Task<bool> removeEvent(Event eventToRemove);
    }
}
