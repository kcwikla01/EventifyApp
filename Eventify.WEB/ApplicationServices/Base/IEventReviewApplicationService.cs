using Eventify.Database.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.WEB.ApplicationServices.Base
{
    public interface IEventReviewApplicationService
    {
        Task<IActionResult> AddEventReview(EventReviewDto eventReviewDto);

    }
}
