using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventify.Database.Models;
using Eventify.Database.Models.Dto;

namespace Eventify.UoW.Base
{
    public interface IManageEventsParticipantsUoW
    {
        Task<EventParticipant> AddEventParticipant(EventParticipantDto eventParticipantDto);
        Task<bool> CheckIfExistLink(EventParticipantDto eventParticipantDto);
        Task<List<EventDto>> GetAllEventsToWhichTheUserIsAssigned(int userId);
        bool RemoveEventParticipant(EventParticipantDto eventParticipantDto);
    }
}
