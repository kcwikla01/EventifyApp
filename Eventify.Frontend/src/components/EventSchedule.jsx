import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "./../styles/_eventSchedulePage.scss";
import eventScheduleTranslations from "./../translations/eventScheduleTranslations";

const EventSchedule = ({ language }) => {
    const { eventId } = useParams();
    const navigate = useNavigate();
    const [activities, setActivities] = useState([]);
    const [error, setError] = useState(null);
    const translations = eventScheduleTranslations[language];

    const fetchActivities = async () => {
        try {
            const response = await fetch(
                `https://localhost:7090/EventShedules/GetAllActivitiesForEvent?eventId=${eventId}`
            );
            if (!response.ok) throw new Error(translations.fetchError);
            const data = await response.json();
            setActivities(data);
        } catch (err) {
            setError(err.message);
        }
    };

    useEffect(() => {
        fetchActivities();
    }, [eventId]);

    return (
        <div className="event-schedule">
            <button className="submit-btn" onClick={() => navigate("/userDashboard")}>
                {language === "pl" ? "Powr�t do panelu u�ytkownika" : "Back to Dashboard"}
            </button>
            <div className="schedule-container">
                <h1>{translations.pageTitle}</h1>

                {error && <p className="error-message">{error}</p>}

                <div className="activities-list">
                    {activities.length > 0 ? (
                        <ul>
                            {activities.map((activity) => (
                                <li key={activity.activityId} className="activity-item">
                                    <h3>{activity.activityName}</h3>
                                    <p>{activity.activityDescription}</p>
                                    <p>
                                        {new Date(activity.startTime).toLocaleString()} -{" "}
                                        {new Date(activity.endTime).toLocaleString()}
                                    </p>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p>{translations.noActivities}</p>
                    )}
                </div>
            </div>
        </div>
    );
};

export default EventSchedule;
