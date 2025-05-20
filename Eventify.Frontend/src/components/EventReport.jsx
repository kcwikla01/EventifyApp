import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { jsPDF } from "jspdf";
import "./../styles/_eventReport.scss";
import eventReportTranslations from "./../translations/eventReportTranslations";

const EventReport = ({ language }) => {
    const { eventId } = useParams();
    const [eventDetails, setEventDetails] = useState(null);
    const [error, setError] = useState(null);
    const translations = eventReportTranslations[language];

    useEffect(() => {
        const fetchEventReport = async () => {
            try {
                const response = await fetch(
                    `https://localhost:7090/EventReport/GenerateReport?eventId=${eventId}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ eventId }),
                }
                );

                if (!response.ok) {
                    throw new Error("Failed to fetch event report");
                }

                const data = await response.json();
                setEventDetails(data);
            } catch (err) {
                setError(err.message);
            }
        };

        fetchEventReport();
    }, [eventId]);

    const handleDownloadPDF = () => {
        if (!eventDetails) return;

        const doc = new jsPDF();

        doc.text(`Event Name: ${eventDetails.eventName}`, 10, 10);
        doc.text(`Description: ${eventDetails.eventDescription}`, 10, 20);
        doc.text(`Start Time: ${new Date(eventDetails.startTime).toLocaleString()}`, 10, 30);
        doc.text(`End Time: ${new Date(eventDetails.endTime).toLocaleString()}`, 10, 40);
        doc.text(`Participants: ${eventDetails.countOfParticipants}`, 10, 50);
        doc.text(`Average Rate: ${eventDetails.averageRate}`, 10, 60);

        if (eventDetails.comments && eventDetails.comments.length > 0) {
            doc.text("Comments:", 10, 70);
            eventDetails.comments.forEach((comment, index) => {
                const commentText = `${index + 1}. ${comment}`;
                doc.text(commentText, 10, 80 + index * 10);
            });
        } else {
            doc.text("No comments available.", 10, 70);
        }
        doc.save(`Event_${eventId}_Report.pdf`);
    };

    if (error) {
        return <p className="error-message">{translations.errorFetchingReport}</p>;
    }

    if (!eventDetails) {
        return <p>{translations.loading}</p>;
    }

    return (
        <div className="event-report">
            <div className="report-container">
                <h1 className="report-header">{translations.eventReportTitle}</h1>
                <div className="report-content">
                    <div className="event-details">
                        <h2>{eventDetails.eventName}</h2>
                        <p>{eventDetails.eventDescription}</p>
                        <p>{translations.startTime}: {new Date(eventDetails.startTime).toLocaleString()}</p>
                        <p>{translations.endTime}: {new Date(eventDetails.endTime).toLocaleString()}</p>
                        <p>{translations.participants}: {eventDetails.countOfParticipants}</p>
                        <p>{translations.averageRating}: {eventDetails.averageRate}</p>
                    </div>
                    <div className="comments-container">
                        <h3>{translations.comments}</h3>
                        {eventDetails.comments && eventDetails.comments.length > 0 ? (
                            <ul>
                                {eventDetails.comments.map((comment, index) => (
                                    <li key={index}>{comment}</li>
                                ))}
                            </ul>
                        ) : (
                            <p>{translations.noComments}</p>
                        )}
                    </div>
                </div>
                <button className="download-pdf-btn" onClick={handleDownloadPDF}>
                    {translations.downloadPDFButton}
                </button>

                <button className="back-to-dashboard-btn" onClick={() => window.history.back()}>
                    {translations.backToDashboardButton}
                </button>
            </div>
        </div>
    );
};

export default EventReport;
