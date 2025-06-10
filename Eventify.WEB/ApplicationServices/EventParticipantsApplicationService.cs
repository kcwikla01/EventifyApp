using Eventify.Database.Models.Dto;
using Eventify.UoW.Base;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices
{
    public class EventParticipantsApplicationService : IEventParticipantsApplicationService
    {
        private readonly IManageEventsParticipantsUoW _manageEventsParticipantsUoW;
        private readonly IManageEventsUoW _manageEventsUoW;
        private readonly IManageUsersUoW _manageUsersUoW;

        public EventParticipantsApplicationService(IManageEventsParticipantsUoW manageEventsParticipantsUoW, IManageEventsUoW manageEventsUoW, IManageUsersUoW manageUsersUoW)
        {
            _manageEventsParticipantsUoW = manageEventsParticipantsUoW;
            _manageEventsUoW = manageEventsUoW;
            _manageUsersUoW = manageUsersUoW;
        }

        public async Task<IActionResult> AddEventParticipant(EventParticipantDto eventParticipantDto, int userId)
        {
            if(eventParticipantDto.UserId != userId)
            {
                return new BadRequestObjectResult("User ID does not match the authenticated user.");
            }
            var checkIfExistThisLink = await _manageEventsParticipantsUoW.CheckIfExistLink(eventParticipantDto);
            if (checkIfExistThisLink)
            {
                return new BadRequestObjectResult("Link exist");
            }

            var ifExistEventAndUser = await _manageEventsParticipantsUoW.CheckIfExistEventAndUser(eventParticipantDto);
            if (!ifExistEventAndUser)
            {
                return new BadRequestObjectResult("Event or user not exist");
            }

            var findedEvent = await _manageEventsUoW.GetEventById(eventParticipantDto.EventId);
            if (findedEvent.OwnerId.Equals(eventParticipantDto.UserId))
            {
                return new BadRequestObjectResult("You are the owner of this event");
            }

            var eventParticipant = await _manageEventsParticipantsUoW.AddEventParticipant(eventParticipantDto);
            if (eventParticipant != null)
            {
                return new OkObjectResult("Linked");
            }

            return new BadRequestObjectResult("Failed to add event participant.");
        }

        public async Task<IActionResult> GetAllEventsToWhichTheUserIsAssigned(int userId)
        {
            var events = await _manageEventsParticipantsUoW.GetAllEventsToWhichTheUserIsAssigned(userId);

            return new OkObjectResult(events);
        }

        public async Task<IActionResult> RemoveEventParticipant(EventParticipantDto eventParticipantDto, int userId)
        {
            var checkIfExistThisLink = await _manageEventsParticipantsUoW.CheckIfExistLink(eventParticipantDto);
            if (checkIfExistThisLink)
            {
                var findedEvent = await _manageEventsUoW.GetEventById(eventParticipantDto.EventId);
                var findedUser = await _manageUsersUoW.GetUserById(userId);
                if (findedEvent == null && (eventParticipantDto.UserId != userId || findedUser.RoleId != 1))
                {
                    return new UnauthorizedObjectResult("You are not authorized to leave this event");
                }
                var removed =  _manageEventsParticipantsUoW.RemoveEventParticipant(eventParticipantDto);
                if(removed)
                {
                    return new OkObjectResult("Link removed");
                }
                else
                {
                    return new BadRequestObjectResult("Link not removed");
                }
            }
            else
            {
                return new BadRequestObjectResult("Link not exist");
            }
        }
    }
}
