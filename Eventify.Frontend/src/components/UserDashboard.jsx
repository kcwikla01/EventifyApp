import React, { useEffect, useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import "./../styles/_userDashboard.scss";
import userDashboardTranslations from "./../translations/userDashboardTranslations";
import { jsPDF } from "jspdf";


const UserDashboard = ({ language }) => {
    const [events, setEvents] = useState([]);
    const [userEvents, setUserEvents] = useState([]);
    const [joinedEvents, setJoinedEvents] = useState([]);
    const [error, setError] = useState(null);
    const navigate = useNavigate();
    const translations = userDashboardTranslations[language];
    const ownerId = localStorage.getItem("userId");

    const extendSession = () => {
        const sessionDuration = 15 * 60 * 1000;
        const expiresAt = Date.now() + sessionDuration;
        localStorage.setItem("expiresAt", expiresAt);
    };

    useEffect(() => {
        const expiresAt = localStorage.getItem("expiresAt");
        if (!expiresAt || Date.now() > parseInt(expiresAt)) {
            localStorage.removeItem("userId");
            localStorage.removeItem("role");
            localStorage.removeItem("expiresAt");
            navigate("/login");
        }

        const events = ["mousemove", "keydown", "scroll", "click"];
        const activityListener = () => {
            extendSession();
        };
        events.forEach((event) => window.addEventListener(event, activityListener));
        return () => {
            events.forEach((event) => window.removeEventListener(event, activityListener));
        };
    }, [navigate]);

    const fetchUserEvents = useCallback(async () => {
        try {
            const response = await fetch(`https://localhost:7090/Event/GetEventsByOwnerId?ownerId=${ownerId}`, {
                headers: {
                    "user-id": ownerId,
                },
            });
            if (!response.ok) {
                throw new Error("Failed to fetch user events");
            }
            const data = await response.json();
            setUserEvents(data);
        } catch (err) {
            setError(err.message);
        }
    }, [ownerId]);


    const fetchAllEvents = async () => {
        try {
            const response = await fetch(
                `https://localhost:7090/Event/GetEvents`,
                {
                    method: 'GET',
                    headers: {
                        "Content-Type": "application/json",
                        "user-id": ownerId,
                    },
                }
            );
            if (!response.ok) {
                throw new Error("Failed to fetch events");
            }
            const data = await response.json();
            setEvents(data);
        } catch (err) {
            setError(err.message);
        }
    };


    const fetchJoinedEvents = async () => {
        try {
            const response = await fetch(
                `https://localhost:7090/EventParticipants/GetAllEventsToWhichTheUserIsAssigned?userId=${ownerId}`,
                {
                    method: 'GET',
                    headers: {
                        "Content-Type": "application/json",
                        "user-id": ownerId,
                    },
                }
            );
            if (!response.ok) {
                throw new Error("Failed to fetch joined events");
            }
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
                    "user-id": ownerId,

                },
                body: JSON.stringify({ userId: parseInt(ownerId), eventId }),
            });
            if (!response.ok) {
                throw new Error("Failed to join event");
            }
            await fetchJoinedEvents();
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
                    "user-id": ownerId,

                },
                body: JSON.stringify({ eventId }),
            });

            if (!response.ok) {
                throw new Error("Failed to generate report");
            }

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
                    const commentText = `${index + 1}. ${comment}`;
                    doc.text(commentText, 10, 80 + index * 10);
                });
            } else {
                doc.text("No comments available", 10, 70);
            }
            doc.save(`Event_${eventId}_Report.pdf`);
        } catch (err) {
            setError(err.message);
        }
    };



    const leaveEvent = async (eventId) => {
        try {
            const response = await fetch(`https://localhost:7090/EventParticipants/RemoveEventParticipant`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                    "user-id": ownerId,

                },
                body: JSON.stringify({ userId: parseInt(ownerId), eventId }),
            });
            if (!response.ok) {
                throw new Error("Failed to leave event");
            }
            await fetchJoinedEvents();
        } catch (err) {
            setError(err.message);
        }
    };

    useEffect(() => {
        fetchAllEvents();
        fetchUserEvents();
        fetchJoinedEvents();
    }, [ownerId, fetchUserEvents]);

    const handleAddEventClick = () => {
        navigate("/addEvent");
    };
    const handleViewReport = (eventId) => {
        navigate(`/event-report/${eventId}`);
    };



    const handleRemoveEvent = async (id) => {
        if (!window.confirm(translations.deleteEventConfirmMessage)) return;
        try {
            const response = await fetch(`https://localhost:7090/Event/RemoveEventById?id=${id}`, {
                method: "DELETE",
                headers: {
                    "user-id": ownerId,
                },
            });
            if (!response.ok) {
                throw new Error("Failed to remove event");
            }
            fetchUserEvents();
            fetchAllEvents();
        } catch (err) {
            setError(err.message);
        }
    };


    const isEventJoined = (eventId) => {
        return joinedEvents.some(e => e.id === eventId);
    };

    const handleViewSchedule = (eventId) => {
        navigate(`/eventSchedule/${eventId}`);
    };

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
                            {events.length > 0 ? (
                                <ul>
                                    {events
                                        .filter(event =>
                                            event.ownerId !== parseInt(ownerId) &&
                                            new Date(event.endDate) > new Date()
                                        )
                                        .map((event) => (
                                            <li key={event.id} className="event-item">
                                                <h3 className="event-name">{event.name}</h3>
                                                <p className="event-description">{event.description}</p>
                                                <p className="event-date">
                                                    {new Date(event.startDate).toLocaleString()} -{" "}
                                                    {new Date(event.endDate).toLocaleString()}
                                                </p>
                                                {!isEventJoined(event.id) && (
                                                    <button className="view-details-btn" onClick={() => joinEvent(event.id)}>
                                                        {translations.joinButton}
                                                    </button>
                                                )}
                                                <button className="view-schedule-btn" onClick={() => handleViewSchedule(event.id)}>
                                                    {translations.viewScheduleButton}
                                                </button>
                                            </li>
                                        ))}
                                </ul>
                            ) : (
                                <p>{translations.noEventsFound}</p>
                            )}
                        </div>
                    </div>

                    <div className="right-column">
                        <div className="user-events-section">
                            <h2 className="section-title">{translations.userEventsTitle}</h2>
                            {userEvents.length > 0 ? (
                                <ul>
                                    {userEvents.map((event) => {
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

export default UserDashboard;
