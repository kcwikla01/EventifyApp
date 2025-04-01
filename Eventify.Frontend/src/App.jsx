import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './components/HomePage';
import RegisterPage from './components/RegisterPage';
import LoginPage from './components/LoginPage';
import UserDashboard from './components/UserDashboard';
import englishFlag from './assets/eng.png';
import polishFlag from './assets/pol.png';
import './App.scss';

function App() {
    const [isDarkMode, setIsDarkMode] = useState(false);
    const [language, setLanguage] = useState('en');

    useEffect(() => {
        const savedMode = localStorage.getItem('darkMode');
        if (savedMode) {
            setIsDarkMode(savedMode === 'true');
        }

        const savedLanguage = localStorage.getItem('language');
        if (savedLanguage) {
            setLanguage(savedLanguage);
        }
    }, []);

    useEffect(() => {
        document.body.classList.toggle('dark-mode', isDarkMode);
        localStorage.setItem('darkMode', isDarkMode);
    }, [isDarkMode]);

    const toggleTheme = () => {
        setIsDarkMode(!isDarkMode);
    };

    const changeLanguage = (lang) => {
        console.log(`Changing language to: ${lang}`);
        setLanguage(lang);
        localStorage.setItem('language', lang);
    };

    return (
        <Router>
            <div className="App">
                <div className="menu-bar">
                    <h1 className="menu-bar__title">Eventify</h1>
                    <div className="theme-toggle">
                        <div className="icon-container">
                            {isDarkMode ? (
                                <i className="fas fa-moon"></i>
                            ) : (
                                <i className="fas fa-sun"></i>
                            )}
                        </div>
                        <label className="switch">
                            <input
                                type="checkbox"
                                checked={isDarkMode}
                                onChange={toggleTheme}
                            />
                            <span className="slider"></span>
                        </label>
                    </div>
                </div>

                {/* Language Switch placed outside the menu-bar */}
                <div className="language-switch">
                    <div onClick={() => changeLanguage('en')} className="language-option">
                        <img
                            src={englishFlag}
                            alt="English"
                            className="flag"
                        />
                    </div>
                    <div onClick={() => changeLanguage('pl')} className="language-option">
                        <img
                            src={polishFlag}
                            alt="Polski"
                            className="flag"
                        />
                    </div>
                </div>

                <Routes>
                    <Route path="/" element={<HomePage language={language} />} />
                    <Route path="/register" element={<RegisterPage language={language} />} />
                    <Route path="/login" element={<LoginPage language={language} />} />
                    <Route path="/userDashboard" element={<UserDashboard language={language} />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
