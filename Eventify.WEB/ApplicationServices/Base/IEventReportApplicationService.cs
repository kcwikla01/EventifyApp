using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices.Base
{
    public interface IEventReportApplicationService
    {
        Task<IActionResult> GenerateReport(int eventId, int userId);
    }
}
