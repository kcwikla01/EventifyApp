﻿import React, { useState, useEffect } from 'react';
import { Routes, Route, Link, useNavigate } from 'react-router-dom';
import HomePage from './components/HomePage';
import RegisterPage from './components/RegisterPage';
import LoginPage from './components/LoginPage';
import UserDashboard from './components/UserDashboard';
import AddEvent from './components/AddEvent';
import UpdateEvent from './components/UpdateEvent';
import ManageSchedule from './components/ManageSchedule';
import AddEventSchedule from './components/AddEventSchedule';
import EventSchedule from './components/EventSchedule';
import EventReviewPage from './components/EventReviewPage';
import EventReport from './components/EventReport';
import AdminDashboard from './components/AdminDashboard';
import englishFlag from './assets/eng.png';
import polishFlag from './assets/pol.png';
import './App.scss';

function App() {
    const navigate = useNavigate();

    const [isDarkMode, setIsDarkMode] = useState(null);
    const [language, setLanguage] = useState('en');
    const [isServerAvailable, setIsServerAvailable] = useState(null);
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    useEffect(() => {
        const userId = localStorage.getItem("userId");
        setIsLoggedIn(!!userId);
    }, []);

    useEffect(() => {
        const handleStorageChange = () => {
            const userId = localStorage.getItem("userId");
            setIsLoggedIn(!!userId);
        };

        window.addEventListener('storage', handleStorageChange);
        return () => window.removeEventListener('storage', handleStorageChange);
    }, []);

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
        const handleUserLoggedIn = () => {
            const userId = localStorage.getItem("userId");
            setIsLoggedIn(!!userId);
        };

        window.addEventListener('userLoggedIn', handleUserLoggedIn);
        return () => window.removeEventListener('userLoggedIn', handleUserLoggedIn);
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

    const handleLogout = () => {
        localStorage.removeItem("userId");
        localStorage.removeItem("role");
        localStorage.removeItem("expiresAt");
        setIsLoggedIn(false);
        navigate("/");
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
        const interval = setInterval(() => {
            const expiresAt = localStorage.getItem("expiresAt");
            const userId = localStorage.getItem("userId");

            if (!expiresAt || new Date().getTime() > Number(expiresAt)) {
                if (userId) {
                    localStorage.removeItem("userId");
                    localStorage.removeItem("role");
                    localStorage.removeItem("expiresAt");
                    setIsLoggedIn(false);

                    if (location.pathname !== '/login') {
                        navigate('/login');
                    }
                }
            } else {
                setIsLoggedIn(!!userId);
            }
        }, 10000);

        return () => clearInterval(interval);
    }, [location.pathname, navigate]);


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

                {isLoggedIn && (
                    <button className="logout-button" onClick={handleLogout}>
                        {language === 'pl' ? 'Wyloguj' : 'Logout'}
                    </button>
                )}
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
                <Route path="/eventReview/:eventId" element={<EventReviewPage language={language} />} />
                <Route path="/event-report/:eventId" element={<EventReport language={language} />} />
                <Route path="/adminDashboard" element={<AdminDashboard language={language} />} />
            </Routes>
        </div>
    );
}

export default App;
