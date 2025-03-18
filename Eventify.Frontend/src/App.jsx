import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './components/HomePage';
import './App.scss';

function App() {
    const [isDarkMode, setIsDarkMode] = useState(false);

    useEffect(() => {
        const savedMode = localStorage.getItem('darkMode');
        if (savedMode) {
            setIsDarkMode(savedMode === 'true');
        }
    }, []);

    useEffect(() => {
        document.body.classList.toggle('dark-mode', isDarkMode);
        localStorage.setItem('darkMode', isDarkMode);
    }, [isDarkMode]);

    // Funkcja do prze³¹czania trybu
    const toggleTheme = () => {
        setIsDarkMode(!isDarkMode);
    };

    return (
        <Router>
            <div className="App">
                <div className="theme-toggle">
                    <label className="switch">
                        <input
                            type="checkbox"
                            checked={isDarkMode}
                            onChange={toggleTheme}
                        />
                        <span className="slider">
                            <div className="icon-container">
                                {/* Ikona w suwaku */}
                                {isDarkMode ? (
                                    <i className="fas fa-moon"></i>
                                ) : (
                                    <i className="fas fa-sun"></i>
                                )}
                            </div>
                        </span>
                    </label>
                </div>

                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/register" element={<div>Register Page</div>} />
                    <Route path="/login" element={<div>Login Page</div>} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
