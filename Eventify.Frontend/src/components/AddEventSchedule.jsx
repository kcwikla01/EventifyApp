import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import "./../styles/_addEventPage.scss";
import addEventTranslations from "./../translations/addEventTranslations";

const AddEventSchedule = ({ language }) => {
    const [activityName, setActivityName] = useState("");
    const [activityDescription, setActivityDescription] = useState("");
    const [startTime, setStartTime] = useState("");
    const [endTime, setEndTime] = useState("");
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const navigate = useNavigate();
    const { eventId } = useParams();
    const translations = addEventTranslations[language];
    const currentDate = new Date().toISOString().slice(0, 16);

    useEffect(() => {
        const userId = localStorage.getItem("userId");
        if (!userId) {
            navigate("/login");
        }
    }, [navigate]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        const ownerId = localStorage.getItem("userId");


        if (!activityName || !startTime || !endTime || !eventId) {
            setError(translations.errorFillFields);
            setSuccess(null);
            return;
        }

        if (new Date(startTime) >= new Date(endTime)) {
            setError(translations.errorDate);
            setSuccess(null);
            return;
        }

        const activityDto = {
            activityId: 0,
            eventId: parseInt(eventId),
            activityName,
            activityDescription,
            startTime,
            endTime,
        };

        try {
            const response = await fetch(`https://localhost:7090/EventShedules/AddEventActivity`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "user-id": ownerId,

                },
                body: JSON.stringify(activityDto),
            });

            if (!response.ok) {
                const message = await response.text();
                throw new Error(message);
            }

            setSuccess(translations.successMessage);
            setError(null);

            setTimeout(() => {
                navigate(`/manageSchedule/${eventId}`);
            }, 1500);

        } catch (err) {
            setError(err.message || translations.serverError);
            setSuccess(null);
        }
    };


    return (
        <div className="add-event-page">
            <div className="event-container">
                <h1 className="form-title">
                    {language === "pl" ? "Dodaj Harmonogram" : "Add Event Schedule"}
                </h1>
                <p className="form-subtitle">
                    {language === "pl"
                        ? "Wprowadź szczegóły aktywności wydarzenia"
                        : "Enter event activity details"}
                </p>

                {error && <p className="error-message">{error}</p>}
                {success && <p className="success-message">{success}</p>}

                <form className="event-form" onSubmit={handleSubmit}>
                    <div className="input-group">
                        <label htmlFor="activityName" className="input-label">
                            {language === "pl" ? "Nazwa aktywności" : "Activity Name"}
                        </label>
                        <input
                            id="activityName"
                            type="text"
                            className="input-field"
                            value={activityName}
                            onChange={(e) => setActivityName(e.target.value)}
                            required
                        />
                    </div>

                    <div className="input-group">
                        <label htmlFor="activityDescription" className="input-label">
                            {language === "pl" ? "Opis aktywności" : "Activity Description"}
                        </label>
                        <textarea
                            id="activityDescription"
                            className="input-field"
                            value={activityDescription}
                            onChange={(e) => setActivityDescription(e.target.value)}
                            rows={4}
                        />
                    </div>

                    <div className="input-group">
                        <label htmlFor="startTime" className="input-label">
                            {language === "pl" ? "Czas rozpoczęcia" : "Start Time"}
                        </label>
                        <input
                            id="startTime"
                            type="datetime-local"
                            className="input-field"
                            value={startTime}
                            onChange={(e) => setStartTime(e.target.value)}
                            min={currentDate}
                            required
                        />
                    </div>

                    <div className="input-group">
                        <label htmlFor="endTime" className="input-label">
                            {language === "pl" ? "Czas zakończenia" : "End Time"}
                        </label>
                        <input
                            id="endTime"
                            type="datetime-local"
                            className="input-field"
                            value={endTime}
                            onChange={(e) => setEndTime(e.target.value)}
                            min={startTime || currentDate}
                            required
                        />
                    </div>

                    <button type="submit" className="submit-btn">
                        {language === "pl" ? "Dodaj aktywność" : "Add Activity"}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default AddEventSchedule;
