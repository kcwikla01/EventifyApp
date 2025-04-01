import React, { useState } from "react";
import "./../styles/_registerPage.scss";
import registerTranslations from './../translations/registerTranslations';
import CryptoJS from "crypto-js";

const RegisterPage = ({ language }) => {
    const [formData, setFormData] = useState({
        username: "",
        email: "",
        password: "",
        confirmPassword: "",
    });
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);
    const [passwordMatch, setPasswordMatch] = useState(true);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });

        if (e.target.name === "confirmPassword") {
            setPasswordMatch(e.target.value === formData.password);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!passwordMatch) {
            setError(registerTranslations[language].passwordMismatchError);
            return;
        }

        // Generowanie deterministycznego has�a za pomoc� SHA-256
        const hashedPassword = CryptoJS.SHA256(formData.password).toString(CryptoJS.enc.Base64);

        // Przygotowanie danych do wys�ania do backendu
        const userDto = {
            name: formData.username,
            email: formData.email,
            password: hashedPassword,
        };

        try {
            const response = await fetch('https://localhost:7090/user/createuser', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(userDto),
            });

            if (!response.ok) {
                const errorData = await response.json();
                setError(errorData.message || "An error occurred.");
                setSuccess(null);
            } else {
                const responseData = await response.json();
                setSuccess(`${translations.successMessage} ${responseData.name}!`);
                setError(null);
            }
        } catch (err) {
            console.error("Error during registration:", err);
            setError("An error occurred. Please try again.");
            setSuccess(null);
        }
    };

    const translations = registerTranslations[language];

    return (
        <div className="register-page">
            <div className="register-container">
                <h1 className="register-title">{translations.title}</h1>
                <p className="register-subtitle">{translations.subtitle}</p>

                {/* Poka� komunikaty o b��dach lub sukcesach */}
                {error && <p className="error-message">{error}</p>}
                {success && <p className="success-message">{success}</p>}

                <form className="register-form" onSubmit={handleSubmit}>
                    <div className="input-group">
                        <input
                            type="text"
                            name="username"
                            placeholder={translations.usernamePlaceholder}
                            className="input-field"
                            value={formData.username}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="input-group">
                        <input
                            type="email"
                            name="email"
                            placeholder={translations.emailPlaceholder}
                            className="input-field"
                            value={formData.email}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="input-group">
                        <input
                            type="password"
                            name="password"
                            placeholder={translations.passwordPlaceholder}
                            className="input-field"
                            value={formData.password}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="input-group">
                        <input
                            type="password"
                            name="confirmPassword"
                            placeholder={translations.confirmPasswordPlaceholder}  
                            className="input-field"
                            value={formData.confirmPassword}
                            onChange={handleChange}
                            required
                        />
                        {!passwordMatch && <p className="error-message">{translations.passwordMismatchError}</p>}  {/* Komunikat o b��dzie */}
                    </div>
                    <button type="submit" className="register-btn" disabled={!passwordMatch}>
                        {translations.registerButton}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default RegisterPage;
