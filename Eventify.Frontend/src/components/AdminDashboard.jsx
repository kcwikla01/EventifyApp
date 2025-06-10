import React, { useEffect, useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import userDashboardTranslations from "./../translations/userDashboardTranslations";
import "./../styles/_userDashboard.scss";
import { jsPDF } from "jspdf";

const AdminDashboard = ({ language }) => {
    const [events, setEvents] = useState([]);
    const [adminEvents, setAdminEvents] = useState([]);
    const [joinedEvents, setJoinedEvents] = useState([]);
    const [error, setError] = useState(null);

    const navigate = useNavigate();
    const translations = userDashboardTranslations[language];
    const adminId = localStorage.getItem("userId");
   


    useEffect(() => {
        const expiresAt = localStorage.getItem("expiresAt");
        if (!expiresAt || Date.now() > parseInt(expiresAt)) {
            localStorage.clear();
            navigate("/login");
        }
    }, [navigate]);

    const fetchAllEvents = async () => {
        try {
            const response = await fetch(`https://localhost:7090/Event/GetEvents`, {
                headers: {
                    "Content-Type": "application/json",
                    "user-id": adminId,
                },
            });
            if (!response.ok) throw new Error("Failed to fetch events");
            const data = await response.json();
            setEvents(data);
        } catch (err) {
            setError(err.message);
        }
    };

    const fetchAdminEvents = useCallback(async () => {
        try {
            const response = await fetch(`https://localhost:7090/Event/GetEventsByOwnerId?ownerId=${adminId}`, {
                headers: {
                    "user-id": adminId,
                },
            });
            if (!response.ok) throw new Error("Failed to fetch admin events");
            const data = await response.json();
            setAdminEvents(data);
        } catch (err) {
            setError(err.message);
        }
    }, [adminId]);

    const fetchJoinedEvents = async () => {
        try {
            const response = await fetch(`https://localhost:7090/EventParticipants/GetAllEventsToWhichTheUserIsAssigned?userId=${adminId}`, {
                headers: {
                    "Content-Type": "application/json",
                    "user-id": adminId,
                },
            });
            if (!response.ok) throw new Error("Failed to fetch joined events");
            const data = await response.json();
            setJoinedEvents(data);
        } catch (err) {
            setError(err.message);
        }
    };
    const joinEvent = async (eventId) => {
        try {
            const response = await fetch(`https://localhost:7090/EventParticipants/AddEventParticipant`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "user-id": adminId,
                },
                body: JSON.stringify({ userId: parseInt(adminId), eventId }),
            });
            if (!response.ok) {
                throw new Error("Failed to join event");
            }
            await fetchJoinedEvents();
        } catch (err) {
            setError(err.message);
        }
    };
    const leaveEvent = async (eventId) => {
        try {
            if (!adminId) {
                setError("User ID is missing");
                return;
            }

            const response = await fetch(`https://localhost:7090/EventParticipants/RemoveEventParticipant`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                    "user-id": adminId,
                },
                body: JSON.stringify({
                    userId: parseInt(adminId),
                    eventId: eventId,
                }),
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Failed to leave event: ${response.status} - ${errorText}`);
            }

            await fetchJoinedEvents();
        } catch (err) {
            setError(err.message);
        }
    };


    const handleRemoveEvent = async (id) => {
        if (!window.confirm(translations.deleteEventConfirmMessage)) return;
        try {
            const response = await fetch(`https://localhost:7090/Event/RemoveEventById?id=${id}`, {
                method: "DELETE",
                headers: {
                    "user-id": adminId,
                },
            });
            if (!response.ok) throw new Error("Failed to remove event");
            fetchAllEvents();
            fetchAdminEvents();
            fetchJoinedEvents();
        } catch (err) {
            setError(err.message);
        }
    };

    const handleGenerateReport = async (eventId) => {
        try {
            const response = await fetch(`https://localhost:7090/EventReport/GenerateReport?eventId=${eventId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'user-id': adminId,
                },
                body: JSON.stringify({ eventId }),
            });

            if (!response.ok) throw new Error("Failed to generate report");

            const eventDetails = await response.json();
            const doc = new jsPDF();

            doc.text(`Event Name: ${eventDetails.eventName}`, 10, 10);
            doc.text(`Description: ${eventDetails.eventDescription}`, 10, 20);
            doc.text(`Start Time: ${new Date(eventDetails.startTime).toLocaleString()}`, 10, 30);
            doc.text(`End Time: ${new Date(eventDetails.endTime).toLocaleString()}`, 10, 40);
            doc.text(`Participants: ${eventDetails.countOfParticipants}`, 10, 50);
            doc.text(`Average Rate: ${eventDetails.averageRate}`, 10, 60);

            if (eventDetails.comments.length > 0) {
                doc.text("Comments:", 10, 70);
                eventDetails.comments.forEach((comment, index) => {
                    doc.text(`${index + 1}. ${comment}`, 10, 80 + index * 10);
                });
            } else {
                doc.text("No comments available", 10, 70);
            }

            doc.save(`Event_${eventId}_Report.pdf`);
        } catch (err) {
            setError(err.message);
        }
    };

    const handleManageSchedule = (eventId) => navigate(`/manageSchedule/${eventId}`);
    const handleViewSchedule = (eventId) => navigate(`/eventSchedule/${eventId}`);
    const handleViewReport = (eventId) => navigate(`/event-report/${eventId}`);
    const handleAddEventClick = () => {
        navigate("/addEvent");
    };

    // Pomocnicza funkcja, by sprawdzaæ czy event jest do³¹czony przez usera
    const isEventJoined = (eventId) => {
        return joinedEvents.some(e => e.id === eventId);
    };

    useEffect(() => {
        fetchAllEvents();
        fetchAdminEvents();
        fetchJoinedEvents();
    }, [fetchAdminEvents]);

    return (
        <div className="user-dashboard">
            <div className="dashboard-container">
                <h1 className="dashboard-header">{translations.dashboardTitle}</h1>
                <button className="add-event-btn" onClick={handleAddEventClick}>
                    {translations.addEventButton}
                </button>

                {error && <p className="error-message">{translations.errorFetchingEvents}</p>}

                <div className="sections-wrapper">
                    <div className="left-column">
                        <div className="events-section">
                            <h2 className="section-title">{translations.allEventsTitle}</h2>
                            {events.filter(event => event.ownerId !== parseInt(adminId)).length > 0 ? (
                                <ul>
                                    {events
                                        .filter(event => event.ownerId !== parseInt(adminId))
                                        .map((event) => {
                                            const hasEnded = new Date(event.endDate) <= new Date();
                                            return (
                                                <li key={event.id} className="event-item">
                                                    <h3 className="event-name">{event.name}</h3>
                                                    <p className="event-description">{event.description}</p>
                                                    <p className="event-date">
                                                        {new Date(event.startDate).toLocaleString()} -{" "}
                                                        {new Date(event.endDate).toLocaleString()}
                                                    </p>
                                                    <div className="event-actions">
                                                        {!isEventJoined(event.id) && (
                                                            <button
                                                                className="view-details-btn"
                                                                onClick={() => joinEvent(event.id)}
                                                            >
                                                                {translations.joinButton}
                                                            </button>
                                                        )}
                                                        <button
                                                            className="view-details-btn"
                                                            onClick={() => handleViewSchedule(event.id)}
                                                        >
                                                            {translations.viewScheduleButton}
                                                        </button>
                                                        <button
                                                            className="remove-event-btn"
                                                            onClick={() => handleRemoveEvent(event.id)}
                                                        >
                                                            {translations.removeEventButton}
                                                        </button>
                                                        <button
                                                            className="update-event-btn"
                                                            onClick={() => navigate(`/updateEvent/${event.id}`)}
                                                        >
                                                            {translations.updateEventButton}
                                                        </button>
                                                        {hasEnded ? (
                                                            <>
                                                                <button
                                                                    className="view-details-btn"
                                                                    onClick={() => handleViewReport(event.id)}
                                                                >
                                                                    {translations.viewReportButton}
                                                                </button>
                                                                <button
                                                                    className="generate-report-btn"
                                                                    onClick={() => handleGenerateReport(event.id)}
                                                                >
                                                                    {translations.generateReportButton}
                                                                </button>
                                                            </>
                                                        ) : (
                                                            <button
                                                                className="update-event-btn"
                                                                onClick={() => handleManageSchedule(event.id)}
                                                            >
                                                                {translations.manageScheduleButton}
                                                            </button>
                                                        )}
                                                    </div>
                                                </li>
                                            );
                                        })}
                                </ul>
                            ) : (
                                <p>{translations.noEventsFound}</p>
                            )}

                        </div>
                    </div>

                    <div className="right-column">
                        <div className="user-events-section">
                            <h2 className="section-title">{translations.userEventsTitle}</h2>
                            {adminEvents.length > 0 ? (
                                <ul>
                                    {adminEvents.map((event) => {
                                        const hasEnded = new Date(event.endDate) <= new Date();
                                        return (
                                            <li key={event.id} className="event-item">
                                                <h3 className="event-name">{event.name}</h3>
                                                <p className="event-description">{event.description}</p>
                                                <p className="event-date">
                                                    {new Date(event.startDate).toLocaleString()} -{" "}
                                                    {new Date(event.endDate).toLocaleString()}
                                                </p>
                                                <div className="event-actions">
                                                    {hasEnded ? (
                                                        <>
                                                            <button
                                                                className="remove-event-btn"
                                                                onClick={() => handleRemoveEvent(event.id)}
                                                            >
                                                                {translations.removeEventButton}
                                                            </button>
                                                            <button
                                                                className="view-details-btn"
                                                                onClick={() => handleViewReport(event.id)}
                                                            >
                                                                {translations.viewReportButton}
                                                            </button>
                                                            <button
                                                                className="generate-report-btn"
                                                                onClick={() => handleGenerateReport(event.id)}
                                                            >
                                                                {translations.generateReportButton}
                                                            </button>
                                                        </>
                                                    ) : (
                                                        <>
                                                            <button
                                                                className="update-event-btn"
                                                                onClick={() => navigate(`/updateEvent/${event.id}`)}
                                                            >
                                                                {translations.updateEventButton}
                                                            </button>
                                                            <button
                                                                className="remove-event-btn"
                                                                onClick={() => handleRemoveEvent(event.id)}
                                                            >
                                                                {translations.removeEventButton}
                                                            </button>
                                                            <button
                                                                className="view-details-btn"
                                                                onClick={() => navigate(`/manageSchedule/${event.id}`)}
                                                            >
                                                                {translations.manageScheduleButton}
                                                            </button>
                                                        </>
                                                    )}
                                                </div>
                                            </li>
                                        );
                                    })}
                                </ul>
                            ) : (
                                <p>{translations.noUserEvents}</p>
                            )}
                        </div>

                        <div className="joined-events-section">
                            <h2 className="section-title">{translations.joinedEventsTitle}</h2>
                            {joinedEvents.length > 0 ? (
                                <ul>
                                    {joinedEvents.map((event) => {
                                        const hasEnded = new Date(event.endDate) <= new Date();
                                        return (
                                            <li key={event.id} className="event-item">
                                                <h3 className="event-name">{event.name}</h3>
                                                <p className="event-description">{event.description}</p>
                                                <p className="event-date">
                                                    {new Date(event.startDate).toLocaleString()} -{" "}
                                                    {new Date(event.endDate).toLocaleString()}
                                                </p>
                                                <div className="event-actions">
                                                    <button
                                                        className="remove-event-btn"
                                                        onClick={() => leaveEvent(event.id)}
                                                    >
                                                        {translations.leaveEventButton}
                                                    </button>
                                                    {hasEnded && (
                                                        <button
                                                            className="view-details-btn"
                                                            onClick={() => navigate(`/eventReview/${event.id}`)}
                                                        >
                                                            {translations.addReviewButton}
                                                        </button>
                                                    )}
                                                </div>
                                            </li>
                                        );
                                    })}
                                </ul>
                            ) : (
                                <p>{translations.noJoinedEvents}</p>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default AdminDashboard;
