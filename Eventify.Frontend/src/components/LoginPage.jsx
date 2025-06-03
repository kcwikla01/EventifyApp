import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "./../styles/_loginPage.scss";
import loginTranslations from './../translations/loginTranslations';
import CryptoJS from "crypto-js";

const Login = ({ language }) => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(null);

    const navigate = useNavigate();
    const translations = loginTranslations[language];

    useEffect(() => {
        const expiresAt = localStorage.getItem("expiresAt");
        if (expiresAt && Date.now() < parseInt(expiresAt)) {
            const role = localStorage.getItem("role");
            if (role === "Admin") {
                navigate("/adminDashboard");
            } else {
                navigate("/userDashboard");
            }
        }
 else {
            localStorage.removeItem("userId");
            localStorage.removeItem("role");
            localStorage.removeItem("expiresAt");
        }
    }, [navigate]);

    const handleLogin = async (e) => {
        e.preventDefault();

        const hashedPassword = CryptoJS.SHA256(password).toString(CryptoJS.enc.Base64);

        const isEmail = username.includes('@');

        const userDto = {
            name: isEmail ? "" : username,
            email: isEmail ? username : "",
            password: hashedPassword,
        };

        try {
            const response = await fetch('https://localhost:7090/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(userDto),
            });

            if (!response.ok) {
                const errorData = await response.text();
                setError(errorData || translations.loginFailed);
                setSuccess(null);
            } else {
                const data = await response.json();

                const sessionDuration = 15 * 60 * 1000; 
                const expiresAt = Date.now() + sessionDuration;

                localStorage.setItem("userId", data.userId);
                localStorage.setItem("role", data.roleName);
                localStorage.setItem("expiresAt", expiresAt.toString());
                localStorage.setItem("name", data.name);

                setSuccess(`${translations.welcome}, ${username}!`);
                setError(null);

                setTimeout(() => {
                    window.dispatchEvent(new Event("userLoggedIn"));
                    if (data.name === "Admin") {
                        navigate("/adminDashboard");
                    } else {
                        navigate("/userDashboard");
                    }
                }, 1500);

            }
        } catch (err) {
            console.error("Login error:", err);
            setError(translations.serverError);
            setSuccess(null);
        }
    };

    return (
        <div className="login-page">
            <div className="login-container">
                <h1 className="login-title">{translations.title}</h1>
                <p className="login-subtitle">{translations.subtitle}</p>

                {error && <p className="error-message">{error}</p>}
                {success && <p className="success-message">{success}</p>}

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
