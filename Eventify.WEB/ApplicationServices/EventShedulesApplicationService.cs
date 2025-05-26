using System.Diagnostics;
using AutoMapper;
using Eventify.Database.Models;
using Eventify.Database.Models.Dto;
using Eventify.UoW;
using Eventify.UoW.Base;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices
{
    public class EventShedulesApplicationService : IEventShedulesApplicationService
    {
        private readonly IManageEventShedulesUoW _manageEventShedulesUoW;
        private readonly IManageEventsUoW _manageEventsUoW;
        private readonly IManageUsersUoW _manageUsersUoW;
        private readonly IMapper _mapper;
        private int _currentUserId;
        private string? _currentUserRole;
        private bool _userContextInitialized = false;
        private IActionResult? _userContextError;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventShedulesApplicationService(IManageEventShedulesUoW manageEventShedulesUoW, IManageEventsUoW manageEventsUoW, IMapper mapper, IManageUsersUoW manageUsersUoW)
        {
            _manageEventShedulesUoW = manageEventShedulesUoW;
            _manageEventsUoW = manageEventsUoW;
            _mapper = mapper;
            _manageUsersUoW = manageUsersUoW;
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
        public async Task<IActionResult> RemoveActivity(int activityId)
        {
            var eventActivity = await _manageEventShedulesUoW.GetEventActivity(activityId);
            if (eventActivity == null)
            {
                return new BadRequestObjectResult("Activity not found");
            }

            var findedEvent = await _manageEventsUoW.GetEventById(eventActivity.EventId);
            if (findedEvent != null)
            {
                await EnsureUserContext();

                if (_userContextError != null)
                    return _userContextError;

                if (_currentUserRole != "Admin" && _currentUserId != findedEvent.OwnerId)
                    return new UnauthorizedObjectResult("User with id " + _currentUserId + " is not authorized");
            }

            var isActivityRemoved = await _manageEventShedulesUoW.RemoveEventActivity(eventActivity);

            if (isActivityRemoved)
                return new OkObjectResult("Activity is removed");
            return new BadRequestObjectResult("Activity is not removed");
        }

        public async Task<IActionResult> GetAllActivitiesForEvent(int eventId)
        {
             var eventActivities = await _manageEventShedulesUoW.GetEventsActivities(eventId);

             return new OkObjectResult(eventActivities);
        }

        public async Task<IActionResult> GetActivityInfo(int activityId)
        {
            var eventActivity = await _manageEventShedulesUoW.GetEventActivity(activityId);

            return new OkObjectResult(_mapper.Map<EventActivityDto>(eventActivity));
        }

        public async Task<IActionResult> AddEventShedules(EventActivityDto eventActivityDto)
        {
            var findEvent = await _manageEventsUoW.GetEventById(eventActivityDto.EventId);
            if (findEvent == null)
            {
                return new BadRequestObjectResult("Event not found");
            }
            else
            {
                await EnsureUserContext();

                if (_userContextError != null)
                    return _userContextError;

                if (_currentUserRole != "Admin" && _currentUserId != findEvent.OwnerId)
                    return new UnauthorizedObjectResult("User with id " + _currentUserId + " is not authorized");
            }
            var isValidDate = ValidateEventActivityDatetimes(findEvent, eventActivityDto);
            if (!isValidDate)
            {
                return new BadRequestObjectResult("Not valid startTime or EndTime");
            }

            var checkIfHasConflictingActivity =
                await _manageEventShedulesUoW.CheckIfHasConflictingActivity(eventActivityDto);

            if (checkIfHasConflictingActivity)
            {
                return new BadRequestObjectResult("There is already an activity scheduled at the same time");
            }

            var addedShedules = await _manageEventShedulesUoW.AddEventActivity(eventActivityDto);

            if (addedShedules == null)
            {
                return new BadRequestObjectResult("Error while adding event shedules");
            }
            return new OkObjectResult(addedShedules);
        }

        private bool ValidateEventActivityDatetimes(Event findEvent, EventActivityDto eventActivityDto)
        {
            if (eventActivityDto.StartTime < findEvent.StartDate)
            {
                return false;
            }

            if (eventActivityDto.StartTime > findEvent.EndDate)
            {
                return false;
            }

            if (eventActivityDto.EndTime < findEvent.StartDate)
            {
                return false;
            }

            if (eventActivityDto.EndTime > findEvent.EndDate)
            {
                return false;
            }

            if (eventActivityDto.EndTime < eventActivityDto.StartTime)
            {
                return false;
            }

            return true;
        }
    }
}
