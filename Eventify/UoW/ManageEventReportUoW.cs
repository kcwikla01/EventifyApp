using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventify.Database.DbContext;
using Eventify.Database.Models.Dto;
using Eventify.UoW.Base;

namespace Eventify.UoW
{
    public class ManageEventReportUoW : IManageEventReportUoW
    {
        private readonly IManageEventsUoW _manageEvents;
        private readonly IManageEventsReviewUoW _manageEventsReviewUoW;
        private readonly EventifyDbContext _context;
        public ManageEventReportUoW(EventifyDbContext context, IManageEventsUoW manageEvents, IManageEventsReviewUoW manageEventsReviewUoW)
        {
            _context = context;
            _manageEvents = manageEvents;
            _manageEventsReviewUoW = manageEventsReviewUoW;
        }
        public async Task<ReportDTO> GenerateReport(int eventId)
        {
            var findedEvent = await _manageEvents.GetEventById(eventId);

            if (findedEvent == null)
            {
                throw new Exception("Event not exist");
            }

            ReportDTO report = new ReportDTO()
            {
                EventName = findedEvent.Name,
                EventDescription = findedEvent.Description,
                CountOfParticipants = 
                    _context.EventParticipants
                    .Where(x => x.EventId == eventId)
                    .Count(),
                startTime = findedEvent.StartDate,
                endTime = findedEvent.EndDate,
                AverageRate = await _manageEventsReviewUoW.GetAverageRatingForEvent(eventId),
                Comments = await _manageEventsReviewUoW.GetCommentsForEvent(eventId)
            };

            return report;
        }
    }
}
