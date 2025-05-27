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
        private int _currentUserId;
        private string? _currentUserRole;
        private bool _userContextInitialized = false;
        private IActionResult? _userContextError;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventReportApplicationService(IManageEventsUoW manageEventsUoW, IManageEventReportUoW manageEventReportUoW, IManageUsersUoW manageUsersUoW, IHttpContextAccessor httpContextAccessor)
        {
            _manageEventsUoW = manageEventsUoW;
            _manageEventReportUoW = manageEventReportUoW;
            _manageUsersUoW = manageUsersUoW;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> GenerateReport(int eventId)
        {

           var findedEvent = await  _manageEventsUoW.GetEventById(eventId);

            if(findedEvent == null)
            {
                return new NotFoundObjectResult("Event not exist");
            }

            if (_userContextError != null)
                return _userContextError;

            if (_currentUserRole != "Admin" && _currentUserId != findedEvent.OwnerId)
                return new UnauthorizedObjectResult("User with id " + _currentUserId + " is not authorized");

            if (findedEvent.EndDate > DateTime.Now)
            {
                return new BadRequestObjectResult("Cannot generate a report before the event has concluded.");
            }

            var report = await _manageEventReportUoW.GenerateReport(eventId);

            return new OkObjectResult(report);
        }
    }
}
