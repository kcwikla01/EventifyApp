import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import HomePage from './components/HomePage';
import RegisterPage from './components/RegisterPage';
import LoginPage from './components/LoginPage';
import UserDashboard from './components/UserDashboard';
import AddEvent from './components/AddEvent';
import UpdateEvent from './components/UpdateEvent';
import ManageSchedule from './components/ManageSchedule';
import AddEventSchedule from './components/AddEventSchedule';
import EventSchedule from './components/EventSchedule';
import englishFlag from './assets/eng.png';
import polishFlag from './assets/pol.png';
import './App.scss';

function App() {
    const [isDarkMode, setIsDarkMode] = useState(null);
    const [language, setLanguage] = useState('en');
    const [isServerAvailable, setIsServerAvailable] = useState(null);

    useEffect(() => {
        const savedMode = localStorage.getItem('darkMode');
        if (savedMode !== null) {
            const mode = savedMode === 'true';
            setIsDarkMode(mode);
            document.body.classList.toggle('dark-mode', mode);
        } else {
            setIsDarkMode(false);
        }

        const savedLanguage = localStorage.getItem('language');
        if (savedLanguage) {
            setLanguage(savedLanguage);
        }
    }, []);

    useEffect(() => {
        if (isDarkMode !== null) {
            document.body.classList.toggle('dark-mode', isDarkMode);
            localStorage.setItem('darkMode', isDarkMode);
        }
    }, [isDarkMode]);

    const toggleTheme = () => {
        if (isDarkMode !== null) {
            setIsDarkMode(!isDarkMode);
        }
    };

    const changeLanguage = (lang) => {
        setLanguage(lang);
        localStorage.setItem('language', lang);
    };

    const checkServerAvailability = async () => {
        try {
            const response = await fetch('https://localhost:7090/System/PingAnonymous');
            if (response.ok) {
                setIsServerAvailable(true);
            } else {
                setIsServerAvailable(false);
            }
        } catch {
            setIsServerAvailable(false);
        }
    };

    useEffect(() => {
        checkServerAvailability();
    }, []);

    if (isServerAvailable === null) {
        return <div>Loading...</div>;
    }

    if (!isServerAvailable) {
        return <div className="error-message">Server is unavailable. Please try again later.</div>;
    }

    return (
        <Router>
            <div className="App">
                <div className="app-title">
                    <Link to="/">
                        <h1>Eventify</h1>
                    </Link>
                </div>

                <div className="menu-bar">
                    <div className="theme-toggle" onClick={toggleTheme}>
                        <div className={`icon-container ${isDarkMode ? 'dark' : 'light'}`}>
                            {isDarkMode ? (
                                <i className="fas fa-moon"></i>
                            ) : (
                                <i className="fas fa-sun"></i>
                            )}
                        </div>
                    </div>
                </div>

                {/* Przełącznik języka */}
                <div className="language-switch">
                    <div onClick={() => changeLanguage('en')} className="language-option">
                        <img src={englishFlag} alt="English" className="flag" />
                    </div>
                    <div onClick={() => changeLanguage('pl')} className="language-option">
                        <img src={polishFlag} alt="Polski" className="flag" />
                    </div>
                </div>

                <Routes>
                    <Route path="/" element={<HomePage language={language} />} />
                    <Route path="/register" element={<RegisterPage language={language} />} />
                    <Route path="/login" element={<LoginPage language={language} />} />
                    <Route path="/addEvent" element={<AddEvent language={language} />} />
                    <Route path="/userDashboard" element={<UserDashboard language={language} />} />
                    <Route path="/updateEvent/:id" element={<UpdateEvent language={language} />} />
                    <Route path="/manageSchedule/:eventId" element={<ManageSchedule language={language} />} />
                    <Route path="/addEventSchedule/:eventId" element={<AddEventSchedule language={language} />} />
                    <Route path="/eventSchedule/:eventId" element={<EventSchedule language={language} />} />
                </Routes>

            </div>
        </Router>
    );
}

export default App;
