using Eventify.Database.Models.Dto;
using Eventify.UoW;
using Eventify.UoW.Base;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices
{
    public class EventReportApplicationService : IEventReportApplicationService
    {
        private readonly IManageEventsUoW _manageEventsUoW;
        private readonly IManageEventReportUoW _manageEventReportUoW;
        private readonly IManageUsersUoW _manageUsersUoW;


        public EventReportApplicationService(IManageEventsUoW manageEventsUoW, IManageEventReportUoW manageEventReportUoW, IManageUsersUoW manageUsersUoW)
        {
            _manageEventsUoW = manageEventsUoW;
            _manageEventReportUoW = manageEventReportUoW;
            _manageUsersUoW = manageUsersUoW;

        }

        public async Task<IActionResult> GenerateReport(int eventId, int userId)
        {
           var findedEvent = await  _manageEventsUoW.GetEventById(eventId);
           var currentUser = await _manageUsersUoW.GetUserById(userId);

           if (findedEvent.OwnerId != userId && currentUser.RoleId != 1)
           {
               return new UnauthorizedObjectResult("You are not the owner of this event or an admin.");
           }
            
           if(findedEvent == null)
           {
                return new NotFoundObjectResult("Event not exist");
           }

            
           if (findedEvent.EndDate > DateTime.Now)
           {
                return new BadRequestObjectResult("Cannot generate a report before the event has concluded.");
           }

            var report = await _manageEventReportUoW.GenerateReport(eventId);

            return new OkObjectResult(report);
        }
    }
}
