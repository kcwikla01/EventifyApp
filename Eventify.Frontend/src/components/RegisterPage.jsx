import React, { useState } from "react";
import "./../styles/_registerPage.scss";
import registerTranslations from './../translations/registerTranslations';

const RegisterPage = ({ language }) => {
    const [formData, setFormData] = useState({
        username: "",
        email: "",
        password: "",
    });

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log("Registration Data:", formData);
    };

    const translations = registerTranslations[language];

    return (
        <div className="register-page">
            <div className="register-container">
                <h1 className="register-title">{translations.title}</h1>
                <p className="register-subtitle">{translations.subtitle}</p>
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
                    <button type="submit" className="register-btn">
                        {translations.registerButton}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default RegisterPage;
