using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eventify.Database.DbContext;
using Eventify.Database.Models;
using Eventify.Database.Models.Dto;
using Eventify.UoW.Base;
using Microsoft.EntityFrameworkCore;

namespace Eventify.UoW
{
    public class ManageEventsParticipantsUoW : IManageEventsParticipantsUoW
    {
        private readonly EventifyDbContext _context;
        private readonly IManageMailUoW _manageMailUoW;
        private readonly IManageEventsUoW _manageEventsUoW;
        private readonly IMapper _mapper;

        public ManageEventsParticipantsUoW(EventifyDbContext dbContext, IManageMailUoW manageMailUoW, IManageEventsUoW manageEventsUoW, IMapper mapper)
        {
            _context = dbContext;
            _manageMailUoW = manageMailUoW;
            _manageEventsUoW = manageEventsUoW;
            _mapper = mapper;
        }

        public async Task<EventParticipant> AddEventParticipant(EventParticipantDto eventParticipantDto)
        {
            var eventParticipant = new EventParticipant
            {
                EventId = eventParticipantDto.EventId,
                UserId = eventParticipantDto.UserId
            };
            await _context.AddAsync(eventParticipant);
            await _context.SaveChangesAsync();

            await _manageMailUoW.SendEventParticipantEmail(eventParticipantDto.UserId, eventParticipantDto.EventId);

            return eventParticipant;
        }

        public async Task<bool> CheckIfExistLink(EventParticipantDto eventParticipantDto)
        {
            var link = await _context.EventParticipants.FirstOrDefaultAsync(e => e.EventId.Equals(eventParticipantDto.EventId) && e.UserId.Equals(eventParticipantDto.UserId));
            if (link == null)
                return false;
            return true;
        }

        public async Task<List<EventDto>> GetAllEventsToWhichTheUserIsAssigned(int userId)
        {
            var eventsIds = await _context.EventParticipants.Where(e => e.UserId == userId).ToListAsync();

            if (eventsIds.Count == 0)
            {
                return new List<EventDto>();
            }

            var events = new List<EventDto>();
            foreach (var eventId in eventsIds)
            {
                var eventDto = await _manageEventsUoW.GetEventById(eventId.EventId);
                if (eventDto != null)
                {
                    events.Add(_mapper.Map<EventDto>(eventDto));
                }
            }
            return events;
        }

        public bool RemoveEventParticipant(EventParticipantDto eventParticipantDto)
        {
            try
            {
                var findEventParticipant = _context.EventParticipants.FirstOrDefault(e => e.UserId.Equals(eventParticipantDto.UserId) && e.EventId.Equals(eventParticipantDto.EventId));

                _context.EventParticipants.Remove(findEventParticipant);
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
