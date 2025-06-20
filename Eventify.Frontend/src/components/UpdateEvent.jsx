import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import "./../styles/_addEventPage.scss";
import updateEventTranslations from './../translations/updateEventTranslations';

const UpdateEvent = ({ language }) => {
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const navigate = useNavigate();
    const { id } = useParams();
    const translations = updateEventTranslations[language];

    useEffect(() => {
        const userId = localStorage.getItem("userId");
        if (!userId) {
            navigate("/login");
        }
    }, [navigate]);

    useEffect(() => {
        const ownerId = localStorage.getItem("userId");
        const fetchEventData = async () => {
            try {
                const response = await fetch(
                    `https://localhost:7090/Event/GetEventById?id=${id}`,
                    {
                        method: 'GET',
                        headers: {
                            "Content-Type": "application/json",
                            "user-id": ownerId,
                        },
                    }
                );
                if (!response.ok) throw new Error("Failed to fetch event data");
                const data = await response.json();
                setName(data.name);
                setDescription(data.description);
                setStartDate(data.startDate.slice(0, 16));
                setEndDate(data.endDate.slice(0, 16));
            } catch (err) {
                setError(err.message || translations.serverError);
                setSuccess(null);
            }
        };
        fetchEventData();
    }, [id, translations.serverError]);


    const currentDate = new Date().toISOString().slice(0, 16);

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
            id: parseInt(id),
            name,
            description,
            startDate,
            endDate,
            ownerId: parseInt(ownerId)
        };

        try {
            const response = await fetch(`https://localhost:7090/Event/UpdateEventById`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    "user-id": ownerId,

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
                        <label htmlFor="name" className="input-label">
                            {translations.eventNamePlaceholder}
                        </label>
                        <input
                            id="name"
                            type="text"
                            className="input-field"
                            placeholder={translations.eventNamePlaceholder}
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            required
                        />
                    </div>

                    <div className="input-group">
                        <label htmlFor="description" className="input-label">
                            {translations.descriptionPlaceholder}
                        </label>
                        <textarea
                            id="description"
                            className="input-field"
                            placeholder={translations.descriptionPlaceholder}
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                            rows={4}
                        />
                    </div>

                    <div className="input-group">
                        <label htmlFor="startDate" className="input-label">
                            {translations.startDatePlaceholder}
                        </label>
                        <input
                            id="startDate"
                            type="datetime-local"
                            className="input-field"
                            placeholder={translations.startDatePlaceholder}
                            value={startDate}
                            onChange={(e) => setStartDate(e.target.value)}
                            min={currentDate}
                            required
                        />
                    </div>

                    <div className="input-group">
                        <label htmlFor="endDate" className="input-label">
                            {translations.endDatePlaceholder}
                        </label>
                        <input
                            id="endDate"
                            type="datetime-local"
                            className="input-field"
                            placeholder={translations.endDatePlaceholder}
                            value={endDate}
                            onChange={(e) => setEndDate(e.target.value)}
                            min={currentDate}
                            required
                        />
                    </div>

                    <button type="submit" className="submit-btn">
                        {translations.updateEventButton}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default UpdateEvent;
