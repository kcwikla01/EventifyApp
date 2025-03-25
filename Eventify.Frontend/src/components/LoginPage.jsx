import React, { useState } from "react";
import "./../styles/_loginPage.scss";
import loginTranslations from './../translations/loginTranslations';

const Login = ({ language }) => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const handleLogin = (e) => {
        e.preventDefault();
        console.log("Login:", username, password);
        alert(`Logged in as: ${username}`);
    };
    const translations = loginTranslations[language];

    return (
        <div className="login-page">
            <div className="login-container">
                <h1 className="login-title">{translations.title}</h1>
                <p className="login-subtitle">{translations.subtitle}</p>

                <form className="login-form" onSubmit={handleLogin}>
                    <div className="input-group">
                        <input
                            type="text"
                            className="input-field"
                            placeholder={translations.usernamePlaceholder}
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            required
                        />
                    </div>

                    <div className="input-group">
                        <input
                            type="password"
                            className="input-field"
                            placeholder={translations.passwordPlaceholder}
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>

                    <button type="submit" className="login-btn">
                        {translations.loginButton}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default Login;
