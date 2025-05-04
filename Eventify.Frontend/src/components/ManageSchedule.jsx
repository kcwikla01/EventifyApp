import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "./../styles/_manageSchedule.scss";
import manageScheduleTranslations from "./../translations/manageScheduleTranslations";

const ManageSchedule = ({ language }) => {
    const { eventId } = useParams();
    const [activities, setActivities] = useState([]);
    const [activityInfo, setActivityInfo] = useState(null);
    const [error, setError] = useState(null);
    const navigate = useNavigate();
    const translations = manageScheduleTranslations[language];

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

    const fetchActivityInfo = async (activityId) => {
        try {
            const response = await fetch(
                `https://localhost:7090/EventShedules/GetEventActivityInfo?id=${activityId}`
            );
            if (!response.ok) throw new Error(translations.fetchInfoError);
            const data = await response.json();
            setActivityInfo(data);
        } catch (err) {
            setError(err.message);
        }
    };

    const handleRemoveActivity = async (activityId) => {
        if (!window.confirm(translations.removeConfirm)) return;
        try {
            const response = await fetch(
                `https://localhost:7090/EventShedules/RemoveEventActivity?activityId=${activityId}`,
                {
                    method: "DELETE",
                }
            );
            if (!response.ok) throw new Error(translations.removeError);
            fetchActivities();
        } catch (err) {
            setError(err.message);
        }
    };

    useEffect(() => {
        fetchActivities();
    }, [eventId]);

    return (
        <div className="user-dashboard">
            <div className="dashboard-container">
                <h1 className="dashboard-header">{translations.pageTitle}</h1>

                <button className="add-event-btn" onClick={() => navigate("/userdashboard")}>
                    {translations.returnButton}
                </button>

                <button
                    className="add-event-btn"
                    onClick={() => navigate(`/addEventSchedule/${eventId}`)}
                >
                    {translations.addScheduleButton}
                </button>

                {error && <p className="error-message">{error}</p>}

                <div className="sections-wrapper">
                    <div className="left-column">
                        {activityInfo && (
                            <div className="activity-info">
                                <h3>{activityInfo.activityName}</h3>
                                <p>{activityInfo.activityDescription}</p>
                                <p>{new Date(activityInfo.startTime).toLocaleString()} -{" "}
                                    {new Date(activityInfo.endTime).toLocaleString()}</p>
                            </div>
                        )}
                    </div>

                    <div className="right-column">
                        <div className="user-events-section">
                            <h2 className="section-title">{translations.activitiesTitle}</h2>
                            {activities.length > 0 ? (
                                <ul>
                                    {activities.map((activity) => (
                                        <li key={activity.activityId} className="event-item">
                                            <h3
                                                className="event-name"
                                                onClick={() => fetchActivityInfo(activity.activityId)}
                                            >
                                                {activity.activityName}
                                            </h3>
                                            <p className="event-description">
                                                {activity.activityDescription}
                                            </p>
                                            <p className="event-date">
                                                {new Date(activity.startTime).toLocaleString()} -{" "}
                                                {new Date(activity.endTime).toLocaleString()}
                                            </p>
                                            <div className="event-actions">
                                                <button
                                                    className="remove-event-btn"
                                                    onClick={() => handleRemoveActivity(activity.activityId)}
                                                >
                                                    {translations.removeActivityButton}
                                                </button>
                                            </div>
                                        </li>
                                    ))}
                                </ul>
                            ) : (
                                <p>{translations.noActivities}</p>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ManageSchedule;
