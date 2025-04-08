import React, { useEffect, useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import "./../styles/_userDashboard.scss";
import userDashboardTranslations from './../translations/userDashboardTranslations';

const UserDashboard = ({ language }) => {
    const [events, setEvents] = useState([]);
    const [userEvents, setUserEvents] = useState([]);
    const [error, setError] = useState(null);
    const navigate = useNavigate();
    const translations = userDashboardTranslations[language];
    const ownerId = localStorage.getItem("userId");

    const fetchUserEvents = useCallback(async () => {
        try {
            const response = await fetch(`https://localhost:7090/Event/GetEventsByOwnerId?ownerId=${ownerId}`);
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
            const response = await fetch(`https://localhost:7090/Event/GetEvents`);
            if (!response.ok) {
                throw new Error("Failed to fetch events");
            }
            const data = await response.json();
            setEvents(data);
        } catch (err) {
            setError(err.message);
        }
    };

    useEffect(() => {
        fetchAllEvents();
        fetchUserEvents();
    }, [ownerId, fetchUserEvents]);

    const handleAddEventClick = () => {
        navigate("/addEvent");
    };

    return (
        <div className="user-dashboard">
            <div className="dashboard-container">
                <h1 className="dashboard-header">{translations.dashboardTitle}</h1>

                <button className="add-event-btn" onClick={handleAddEventClick}>
                    {translations.addEventButton}
                </button>

                {error && <p className="error-message">{error}</p>}

                <div className="events-section">
                    <h2 className="section-title">{translations.allEventsTitle}</h2>
                    {events.length > 0 ? (
                        <ul>
                            {events.map((event) => (
                                <li key={event.id} className="event-item">
                                    <h3 className="event-name">{event.name}</h3>
                                    <p className="event-description">{event.description}</p>
                                    <p className="event-date">
                                        {new Date(event.startDate).toLocaleString()} -{" "}
                                        {new Date(event.endDate).toLocaleString()}
                                    </p>
                                    <button className="view-details-btn">{translations.joinButton}</button>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p>{translations.noEventsFound}</p>
                    )}
                </div>

                <div className="user-events-section">
                    <h2 className="section-title">{translations.yourEventsTitle}</h2>
                    {userEvents.length > 0 ? (
                        <ul>
                            {userEvents.map((event) => (
                                <li key={event.id} className="event-item">
                                    <h3 className="event-name">{event.name}</h3>
                                    <p className="event-description">{event.description}</p>
                                    <p className="event-date">
                                        {new Date(event.startDate).toLocaleString()} -{" "}
                                        {new Date(event.endDate).toLocaleString()}
                                    </p>
                                    <button className="view-details-btn">{translations.viewDetailsButton}</button>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p>{translations.noUserEvents}</p>
                    )}
                </div>
            </div>
        </div>
    );
};

export default UserDashboard;
