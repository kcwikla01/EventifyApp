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
    public class ManageEventShedulesUoW : IManageEventShedulesUoW
    {
        private readonly EventifyDbContext _eventifyDbContext;
        private readonly IMapper _mapper;

        public ManageEventShedulesUoW(EventifyDbContext eventifyDbContext, IMapper mapper)
        {
            _eventifyDbContext = eventifyDbContext;
            _mapper = mapper;
        }
        public async Task<EventActivityDto> AddEventActivity(EventActivityDto eventActivityDto)
        {
            var eventSchedule = _mapper.Map<EventSchedule>(eventActivityDto);

            _eventifyDbContext.EventSchedules.Add(eventSchedule);
            await _eventifyDbContext.SaveChangesAsync();

            return _mapper.Map<EventActivityDto>(eventSchedule);
        }

        public async Task<bool> RemoveEventActivity(EventSchedule eventSchedule)
        {
            try
            {
                _eventifyDbContext.EventSchedules.Remove(eventSchedule);
                await _eventifyDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<EventSchedule?> GetEventActivity(int activityId)
        {
          var eventActivity =  
              await _eventifyDbContext.EventSchedules.FirstOrDefaultAsync(activity => activity.Id.Equals(activityId));

          return eventActivity;
        }

        public async Task<List<EventActivityDto>> GetEventsActivities(int eventId)
        {
            var eventActivities =
                await _eventifyDbContext.EventSchedules.Where(activity => activity.EventId.Equals(eventId)).ToListAsync();

            return _mapper.Map<List<EventActivityDto>>(eventActivities);
        }

        public async Task<bool> CheckIfHasConflictingActivity(EventActivityDto eventActivityDto)
        {
            var checkIfHasConflict = await _eventifyDbContext.EventSchedules.AnyAsync( activity => activity.EventId == eventActivityDto.EventId
            && ((eventActivityDto.StartTime >= activity.StartTime && eventActivityDto.StartTime < activity.EndTime) ||
                (eventActivityDto.EndTime > activity.StartTime && eventActivityDto.EndTime <= activity.EndTime) ||
                (eventActivityDto.StartTime <= activity.StartTime && eventActivityDto.EndTime >= activity.EndTime)));

            return checkIfHasConflict;
        }
    }
}
