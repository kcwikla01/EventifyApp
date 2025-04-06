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
    public class ManageEventsUoW : IManageEventsUoW
    {
        private readonly EventifyDbContext _context;
        private readonly IMapper _mapper;
        public ManageEventsUoW(EventifyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Event> CreateEvent(EventDto eventDto)
        {
            var newEvent = _mapper.Map<Event>(eventDto);

            try 
            { 
                await _context.Events.AddAsync(newEvent);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return newEvent;
        }

        public async Task<Event?> GetEventById(int id)
        {
           var searchedEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
           
           return searchedEvent;
        }

        public async Task<List<Event>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<List<Event>> GetEventsByOwnerId(int ownerId)
        {
            return await _context.Events.Where(e => e.OwnerId == ownerId).ToListAsync();
        }

        public async Task<bool> removeEvent(Event eventToRemove)
        {
            try
            {
                _context.Events.Remove(eventToRemove);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Event> UpdateEvent(EventDto eventToUpdate)
        {
            var existingEvent = _context.Events.FirstOrDefault(e => e.Id == eventToUpdate.Id);
           
            existingEvent.Name = eventToUpdate.Name;
            existingEvent.Description = eventToUpdate.Description;
            existingEvent.StartDate = eventToUpdate.StartDate;
            existingEvent.EndDate = eventToUpdate.EndDate;
            existingEvent.OwnerId = eventToUpdate.OwnerId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return existingEvent;
        }
    }
}
