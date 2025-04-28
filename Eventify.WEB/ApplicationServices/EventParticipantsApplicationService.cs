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

        public EventParticipantsApplicationService(IManageEventsParticipantsUoW manageEventsParticipantsUoW, IManageEventsUoW manageEventsUoW)
        {
            _manageEventsParticipantsUoW = manageEventsParticipantsUoW;
            _manageEventsUoW = manageEventsUoW;
        }

        public async Task<IActionResult> AddEventParticipant(EventParticipantDto eventParticipantDto)
        {

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

        public async Task<IActionResult> RemoveEventParticipant(EventParticipantDto eventParticipantDto)
        {
            var checkIfExistThisLink = await _manageEventsParticipantsUoW.CheckIfExistLink(eventParticipantDto);
            if (checkIfExistThisLink)
            {
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
