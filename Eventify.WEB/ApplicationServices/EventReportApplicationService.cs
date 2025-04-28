using Eventify.UoW.Base;
using Eventify.WEB.ApplicationServices.Base;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices
{
    public class EventReportApplicationService : IEventReportApplicationService
    {
        private readonly IManageEventsUoW _manageEventsUoW;
        private readonly IManageEventReportUoW _manageEventReportUoW;

        public EventReportApplicationService(IManageEventsUoW manageEventsUoW, IManageEventReportUoW manageEventReportUoW)
        {
            _manageEventsUoW = manageEventsUoW;
            _manageEventReportUoW = manageEventReportUoW;
        }
        public async Task<IActionResult> GenerateReport(int eventId)
        {
           var findedEvent = await  _manageEventsUoW.GetEventById(eventId);

            if(findedEvent == null)
            {
                return new NotFoundObjectResult("Event not exist");
            }

            if(findedEvent.EndDate > DateTime.Now)
            {
                return new BadRequestObjectResult("Cannot generate a report before the event has concluded.");
            }

            var report = await _manageEventReportUoW.GenerateReport(eventId);

            return new OkObjectResult(report);
        }
    }
}
