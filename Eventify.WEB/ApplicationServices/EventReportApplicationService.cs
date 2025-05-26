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

        public async Task<IActionResult> GenerateReport(int eventId)
        {

           var findedEvent = await  _manageEventsUoW.GetEventById(eventId);

            if(findedEvent == null)
            {
                return new NotFoundObjectResult("Event not exist");
            }

            await EnsureUserContext();

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
