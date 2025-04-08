import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./../styles/_addEventPage.scss";
import addEventTranslations from './../translations/addEventTranslations';

const AddEvent = ({ language }) => {
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const navigate = useNavigate();
    const translations = addEventTranslations[language];

    const handleSubmit = async (e) => {
        e.preventDefault();

        const ownerId = localStorage.getItem("userId");

        if (!name || !startDate || !endDate || !ownerId) {
            setError(translations.errorFillFields);
            setSuccess(null);
            return;
        }

        if (new Date(startDate) >= new Date(endDate)) {
            setError(translations.errorDate);
            setSuccess(null);
            return;
        }

        const eventDto = {
            name,
            description,
            startDate,
            endDate,
            ownerId: parseInt(ownerId)
        };

        try {
            const response = await fetch(`https://localhost:7090/Event/CreateEvent`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(eventDto),
            });

            if (!response.ok) {
                const message = await response.text();
                throw new Error(message);
            }

            setSuccess(translations.successMessage);
            setError(null);

            setTimeout(() => {
                navigate("/userDashboard");
            }, 1500);

        } catch (err) {
            setError(err.message || translations.serverError);
            setSuccess(null);
        }
    };

    return (
        <div className="add-event-page">
            <div className="event-container">
                <h1 className="form-title">{translations.formTitle}</h1>
                <p className="form-subtitle">{translations.formSubtitle}</p>

                {error && <p className="error-message">{error}</p>}
                {success && <p className="success-message">{success}</p>}

                <form className="event-form" onSubmit={handleSubmit}>
                    <div className="input-group">
                        <input
                            type="text"
                            className="input-field"
                            placeholder={translations.eventNamePlaceholder}
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            required
                        />
                    </div>

                    <div className="input-group">
                        <textarea
                            className="input-field"
                            placeholder={translations.descriptionPlaceholder}
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                            rows={4}
                        />
                    </div>

                    <div className="input-group">
                        <input
                            type="datetime-local"
                            className="input-field"
                            placeholder={translations.startDatePlaceholder}
                            value={startDate}
                            onChange={(e) => setStartDate(e.target.value)}
                            required
                        />
                    </div>

                    <div className="input-group">
                        <input
                            type="datetime-local"
                            className="input-field"
                            placeholder={translations.endDatePlaceholder}
                            value={endDate}
                            onChange={(e) => setEndDate(e.target.value)}
                            required
                        />
                    </div>

                    <button type="submit" className="submit-btn">
                        {translations.createEventButton}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default AddEvent;
