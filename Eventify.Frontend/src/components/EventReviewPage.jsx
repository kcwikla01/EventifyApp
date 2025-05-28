import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import eventReviewTranslations from "./../translations/eventReviewTranslations";
import "./../styles/_eventReviewPage.scss";

const EventReviewPage = ({ language }) => {
    const { eventId } = useParams();
    const [rating, setRating] = useState(0);
    const [hover, setHover] = useState(0);
    const [comment, setComment] = useState("");
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const navigate = useNavigate();
    const translations = eventReviewTranslations[language];
    const userId = localStorage.getItem("userId");

    useEffect(() => {
        if (!userId) {
            navigate("/login");
        }
    }, [navigate, userId]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        const ownerId = localStorage.getItem("userId");

        if (!rating) {
            setError(translations.ratingError);
            return;
        }

        if (!comment.trim()) {
            setError(translations.commentError);
            return;
        }

        const review = {
            id: 0,
            eventId: parseInt(eventId),
            userId: parseInt(userId),
            rating,
            comment
        };

        try {
            const response = await fetch(`https://localhost:7090/EventReview/AddEventReview`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "user-id": ownerId,

                },
                body: JSON.stringify(review)
            });

            if (!response.ok) {
                const msg = await response.text();
                throw new Error(msg);
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
        <div className="event-review-page">
            <div className="review-container">
                <h1 className="review-title">{translations.reviewTitle}</h1>
                <p className="review-subtitle">{translations.reviewSubtitle}</p>

                {error && <p className="error-message">{error}</p>}
                {success && <p className="success-message">{success}</p>}

                <form className="review-form" onSubmit={handleSubmit}>
                    <div className="stars">
                        {[...Array(5)].map((_, index) => {
                            const current = index + 1;
                            return (
                                <span
                                    key={current}
                                    className={`star ${current <= (hover || rating) ? "filled" : ""}`}
                                    onClick={() => setRating(current)}
                                    onMouseEnter={() => setHover(current)}
                                    onMouseLeave={() => setHover(0)}
                                >
                                    ★
                                </span>
                            );
                        })}
                    </div>

                    <textarea
                        className="input-field"
                        placeholder={translations.commentPlaceholder}
                        rows={4}
                        value={comment}
                        onChange={(e) => setComment(e.target.value)}
                    />

                    <button type="submit" className="submit-btn">
                        {translations.submitButton}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default EventReviewPage;
