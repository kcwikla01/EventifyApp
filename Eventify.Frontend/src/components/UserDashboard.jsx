import React, { useState, useEffect } from "react";
import "./../styles/_userDashboard.scss";
import dashboardTranslations from './../translations/dashboardTranslations';

const UserDashboard = ({ language }) => {
    const [availableEvents, setAvailableEvents] = useState([]);
    const [joinedEvents, setJoinedEvents] = useState([]);


    const translations = dashboardTranslations[language];

    useEffect(() => {




       
        setAvailableEvents([
            { id: 1, name: "Prz1", date: "2025-05-10" },
            { id: 2, name: "Prz2", date: "2025-06-15" },
            { id: 3, name: "Pry3", date: "2025-07-20" }
        ]);

        setJoinedEvents([
            { id: 4, name: "Przy4", date: "2025-04-12" }
        ]);
    }, []);

    const joinEvent = (event) => {
        setJoinedEvents([...joinedEvents, event]);
        setAvailableEvents(availableEvents.filter(e => e.id !== event.id));
    };

    return (
        <div className="user-dashboard">
            <h1 className="dashboard-title">{translations.title}</h1>

            <div className="events-section">
                <h2 className="section-title">{translations.availableEvents}</h2>
                <ul className="events-list">
                    {availableEvents.length > 0 ? availableEvents.map(event => (
                        <li key={event.id} className="event-item">
                            <span>{event.name} - {event.date}</span>
                            <button className="join-btn" onClick={() => joinEvent(event)}>
                                {translations.join}
                            </button>
                        </li>
                    )) : <p>{translations.noAvailableEvents}</p>}
                </ul>
            </div>

            <div className="events-section">
                <h2 className="section-title">{translations.myEvents}</h2>
                <ul className="events-list">
                    {joinedEvents.length > 0 ? joinedEvents.map(event => (
                        <li key={event.id} className="event-item">
                            <span>{event.name} - {event.date}</span>
                        </li>
                    )) : <p>{translations.noJoinedEvents}</p>}
                </ul>
            </div>
        </div>
    );
};

export default UserDashboard;
