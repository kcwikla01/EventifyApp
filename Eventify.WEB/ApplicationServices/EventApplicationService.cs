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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _currentUserId;
        private string? _currentUserRole;
        private bool _userContextInitialized = false;
        private IActionResult? _userContextError;

        public EventApplicationService (IManageEventsUoW manageEventsUoW, IMapper mapper, IManageUsersUoW manageUsersUoW, IHttpContextAccessor httpContextAccessor)
        {
            _manageEventsUoW = manageEventsUoW;
            _manageUsersUoW = manageUsersUoW;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task EnsureUserContext()
        {
            if (_userContextInitialized)
            return;

            var user = _httpContextAccessor.HttpContext?.User;
            var userNameClaim = user?.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name);
            var roleClaim = user?.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role);

            if (userNameClaim == null || string.IsNullOrEmpty(userNameClaim.Value) ||
                roleClaim == null || string.IsNullOrEmpty(roleClaim.Value))
            {
                _userContextError = new UnauthorizedResult();
                _userContextInitialized = true;
                return;
            }

            _currentUserId = await _manageUsersUoW.GetUserByName(userNameClaim.Value);
            _currentUserRole = roleClaim.Value;
            _userContextInitialized = true;
            _userContextError = null;
        }

        public async Task<IActionResult> CreateEvent(EventDto eventDto)
        {
            await EnsureUserContext();
           
            if (_userContextError != null)
                return _userContextError;

            if (_currentUserRole != "Admin" && _currentUserId != eventDto.OwnerId)
                return new UnauthorizedObjectResult("User with id "+ _currentUserId + " is not authorized");

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
            await EnsureUserContext();
         
            if (_userContextError != null)
                return _userContextError;

            var eventById = await _manageEventsUoW.GetEventById(id);

            if (_currentUserRole != "Admin" && _currentUserId != eventById.OwnerId)
                return new UnauthorizedObjectResult("User with id " + _currentUserId + " is not authorized");

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
            await EnsureUserContext();
            var eventById = await _manageEventsUoW.GetEventById(eventDto.Id);

            if (eventById == null)
            {
                return new NotFoundObjectResult("Event not exist");
            }
            
            if (_userContextError != null)
                return _userContextError;

            if (_currentUserRole != "Admin" && _currentUserId != eventById.OwnerId)
                return new UnauthorizedObjectResult("User with id " + _currentUserId + " is not authorized");


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
