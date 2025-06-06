﻿using System.Diagnostics;
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


        public EventShedulesApplicationService(IManageEventShedulesUoW manageEventShedulesUoW, IManageEventsUoW manageEventsUoW, IMapper mapper, IManageUsersUoW manageUsersUoW)
        {
            _manageEventShedulesUoW = manageEventShedulesUoW;
            _manageEventsUoW = manageEventsUoW;
            _manageUsersUoW = manageUsersUoW;
            _mapper = mapper;
        }

        public async Task<IActionResult> RemoveActivity(int activityId, int userId)
        {
            var currentUser = await _manageUsersUoW.GetUserById(userId);
            var eventActivity = await _manageEventShedulesUoW.GetEventActivity(activityId);
            if (eventActivity == null)
            {
                return new BadRequestObjectResult("Activity not found");
            }
            var findedEvent = await _manageEventsUoW.GetEventById(eventActivity.EventId);
            if (findedEvent == null)
            {
                return new BadRequestObjectResult("Event not found");
            }
            if (findedEvent.OwnerId != userId && currentUser.RoleId != 1)
            {
                return new UnauthorizedObjectResult("You are not the owner of this event or an admin");
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

        public async Task<IActionResult> AddEventShedules(EventActivityDto eventActivityDto, int userId)
        {
            var findEvent = await _manageEventsUoW.GetEventById(eventActivityDto.EventId);
            if (findEvent == null)
            {
                return new BadRequestObjectResult("Event not found");
            }

            var currentUser = await _manageUsersUoW.GetUserById(userId);
            if (findEvent.OwnerId != userId && currentUser.RoleId != 1)
            {
                return new UnauthorizedObjectResult("You are not the owner of this event or an admin");
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
