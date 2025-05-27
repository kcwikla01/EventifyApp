using AutoMapper;
using Eventify.Database.Models.Dto;
using Eventify.UoW.Base;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices
{
    public class EventApplicationService : IEventApplicationService
    {
        private readonly IManageEventsUoW _manageEventsUoW;
        private readonly IManageUsersUoW _manageUsersUoW;
        private readonly IMapper _mapper;


        public EventApplicationService(IManageEventsUoW manageEventsUoW, IMapper mapper, IManageUsersUoW manageUsersUoW,)
        {
            _manageEventsUoW = manageEventsUoW;
            _manageUsersUoW = manageUsersUoW;
            _mapper = mapper;
        }



        public async Task<IActionResult> CreateEvent(EventDto eventDto)
        {
            if (eventDto.Validate())
            {
                if (eventDto.ValidateDates())
                {
                    var userEntity = await _manageUsersUoW.GetUserById(eventDto.OwnerId);
                    if (userEntity == null)
                    {
                        return new NotFoundObjectResult("Owner not exist");
                    }
                    try
                    {
                        var newEvent = await _manageEventsUoW.CreateEvent(eventDto);
                        eventDto.Id = newEvent.Id;
                    }
                    catch (System.Exception e)
                    {
                        return new BadRequestObjectResult(e.Message);
                    }
                    return new OkObjectResult(eventDto);
                }
                else
                {
                    return new BadRequestObjectResult("Invalid startDate or endDate");
                }
            }
            else
            {
                return new BadRequestObjectResult("Invalid data");
            }
        }

        public async Task<IActionResult> GetEventById(int id)
        {
            var eventById = await _manageEventsUoW.GetEventById(id);

            if(eventById == null)
            {
                return new NotFoundObjectResult("Event not exist");
            }

            var eventDto = _mapper.Map<EventDto>(eventById);

            return new OkObjectResult(eventDto);
        }

        public async Task<IActionResult> GetEvents()
        {
            var events = await _manageEventsUoW.GetEvents();
            var eventDtos = _mapper.Map<List<EventDto>>(events);

            return new OkObjectResult(eventDtos);
        }

        public async Task<IActionResult> GetEventsByOwnerId(int ownerId)
        {
            var events = await _manageEventsUoW.GetEventsByOwnerId(ownerId);
            var eventDtos = _mapper.Map<List<EventDto>>(events);

            return new OkObjectResult(eventDtos);
        }

        public async Task<IActionResult> RemoveEventById(int id)
        {
            var eventById = await _manageEventsUoW.GetEventById(id);

            if(eventById == null)
            {
                return new NotFoundObjectResult("Event not exist");
            }

            var isRemoved = await _manageEventsUoW.removeEvent(eventById);
            if(isRemoved)
            {
                return new OkObjectResult("Event removed");
            }
            else
            {
                return new BadRequestObjectResult("Event not removed");
            }
        }

        public async Task<IActionResult> UpdateEventById(EventDto eventDto)
        {
            var eventById = await _manageEventsUoW.GetEventById(eventDto.Id);

            if (eventById == null)
            {
                return new NotFoundObjectResult("Event not exist");
            }

            if (eventDto.Validate())
            {
                if (eventDto.ValidateDates())
                {
                    var user = await _manageUsersUoW.GetUserById(eventDto.OwnerId);
                    if (user == null)
                    {
                        return new NotFoundObjectResult("Owner not exist");
                    }

                    var updatedEvent = await _manageEventsUoW.UpdateEvent(eventDto);
                    var updatedEventDto = _mapper.Map<EventDto>(updatedEvent);

                    return new OkObjectResult(updatedEventDto);
                }
                else
                {
                    return new BadRequestObjectResult("Invalid startDate or endDate");
                }
            }
            else
            {
                return new BadRequestObjectResult("Invalid data");
            }

        }
    }
}
