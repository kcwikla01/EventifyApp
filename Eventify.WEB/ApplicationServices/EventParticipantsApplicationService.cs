using Eventify.Database.Models.Dto;
using Eventify.UoW.Base;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices
{
    public class EventParticipantsApplicationService : IEventParticipantsApplicationService
    {
        private readonly IManageEventsParticipantsUoW _manageEventsParticipantsUoW;

        public EventParticipantsApplicationService(IManageEventsParticipantsUoW manageEventsParticipantsUoW)
        {
            _manageEventsParticipantsUoW = manageEventsParticipantsUoW;
        }

        public async Task<IActionResult> AddEventParticipant(EventParticipantDto eventParticipantDto)
        {
            var checkIfExistThisLink = await _manageEventsParticipantsUoW.CheckIfExistLink(eventParticipantDto);
            if (checkIfExistThisLink)
            {
                return new BadRequestObjectResult("Link exist");
            }
            else
            {
                var IfExistEventAndUser = await _manageEventsParticipantsUoW.CheckIfExistEventAndUser(eventParticipantDto);
                if (IfExistEventAndUser)
                {
                    var eventParticipant = await _manageEventsParticipantsUoW.AddEventParticipant(eventParticipantDto);

                    if (eventParticipant != null)
                    {
                        return new OkObjectResult("Linked");
                    }
                    else
                    {
                        return new BadRequestObjectResult("Failed to add event participant.");
                    }
                }
                else
                {
                    return new BadRequestObjectResult("Event or user not exist");
                }
            }
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
