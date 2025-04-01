import React, { useState } from "react";
import { Link } from "react-router-dom";
import "./../styles/_homePage.scss";
import yourImage from "./../assets/image1.jpg";
import "font-awesome/css/font-awesome.min.css";
import translations from "./../translations/home";

const HomePage = ({ language }) => {
    const [contentOrder, setContentOrder] = useState([1, 2, 3]);
    const [isTransitioning, setIsTransitioning] = useState(false);

    const changeContent = (direction) => {
        if (isTransitioning) return;
        setIsTransitioning(true);

        setTimeout(() => {
            setContentOrder((prevOrder) => {
                if (direction === "next") {
                    return [prevOrder[1], prevOrder[2], prevOrder[0]];
                } else {
                    return [prevOrder[2], prevOrder[0], prevOrder[1]];
                }
            });
            setIsTransitioning(false);
        }, 500);
    };

    const contentData = translations[language].content;

    return (
        <div className="home-page">
            <img src={yourImage} alt="Eventify" className="home-page__image" />
            <div className="home-page__container">
                <h1 className="home-page__title">{translations[language].title}</h1>
                <p className="home-page__description">{translations[language].description}</p>
                <div className="home-page__buttons">
                    <Link to="/register">
                        <button className="btn btn--primary">{translations[language].registerButton}</button>
                    </Link>
                    <Link to="/login">
                        <button className="btn btn--secondary">{translations[language].loginButton}</button>
                    </Link>
                </div>
            </div>

            <div className="home-page__about-us">
                <h2 className="home-page__about-us-title">{translations[language].aboutUsTitle}</h2>
                <div className="home-page__content-wrapper">
                    <button className="home-page__arrow left" onClick={() => changeContent("prev")}>
                        <i className="fas fa-arrow-left"></i>
                    </button>

                    <div className="home-page__content">
                        {contentOrder.map((num, index) => (
                            <div key={num} className={`content-container ${index === 1 ? "center" : "side"} ${isTransitioning ? "transitioning" : ""}`}>
                                <h3>{contentData[num].title}</h3>
                                <p>{contentData[num].description}</p>
                            </div>
                        ))}
                    </div>

                    <button className="home-page__arrow right" onClick={() => changeContent("next")}>
                        <i className="fas fa-arrow-right"></i>
                    </button>
                </div>
            </div>
        </div>
    );
};

export default HomePage;
