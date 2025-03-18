import React, { useState} from 'react';
import { Link } from 'react-router-dom';
import './../styles/_homePage.scss';
import yourImage from './../assets/image1.jpg';
import 'font-awesome/css/font-awesome.min.css';

const HomePage = () => {
    const [contentOrder, setContentOrder] = useState([1, 2, 3]);
    const [isTransitioning, setIsTransitioning] = useState(false);

    const changeContent = (direction) => {
        if (isTransitioning) return;
        setIsTransitioning(true);

        setTimeout(() => {
            setContentOrder((prevOrder) => {
                if (direction === 'next') {
                    // Zmiana kolejnoœci na prawo
                    return [prevOrder[1], prevOrder[2], prevOrder[0]];
                } else {
                    // Zmiana kolejnoœci na lewo
                    return [prevOrder[2], prevOrder[0], prevOrder[1]];
                }
            });
            setIsTransitioning(false);
        }, 500);
    };

    const contentData = {
        1: {
            title: "What is Eventify?",
            description: "Eventify is a platform that allows users to create, manage, and join amazing events."
        },
        2: {
            title: "Join Events",
            description: "Explore and join events that match your interests. Connect with people, share experiences, and make new memories."
        },
        3: {
            title: "Event Management",
            description: "Manage your own events effortlessly with our user-friendly platform. Track guests, schedule activities, and much more."
        }
    };

    return (
        <div className="home-page">
            <div className="menu-bar">
                <h1 className="menu-bar__title">Eventify</h1>
                {/* Usuniêty kod zwi¹zany z logowaniem */}
            </div>

            <img src={yourImage} alt="Eventify" className="home-page__image" />

            <div className="home-page__container">
                <h1 className="home-page__title">Welcome to Eventify</h1>
                <p className="home-page__description">A place to create, manage, and join amazing events. Let's get started!</p>
                <div className="home-page__buttons">
                    <Link to="/register"><button className="btn btn--primary">Register</button></Link>
                    <Link to="/login"><button className="btn btn--secondary">Login</button></Link>
                </div>
            </div>

            <div className="home-page__about-us">
                <h2 className="home-page__about-us-title">More about us</h2>
                <div className="home-page__arrow" onClick={() => changeContent('prev')}>
                    <i className="fas fa-arrow-left"></i>
                </div>

                <div className="home-page__content">
                    {contentOrder.map((num, index) => (
                        <div key={num} className={`content-container ${index === 1 ? 'center' : 'side'} ${isTransitioning ? 'transitioning' : ''}`}>
                            <h3>{contentData[num].title}</h3>
                            <p>{contentData[num].description}</p>
                        </div>
                    ))}
                </div>

                <div className="home-page__arrow" onClick={() => changeContent('next')}>
                    <i className="fas fa-arrow-right"></i>
                </div>
            </div>
        </div>
    );
};

export default HomePage;
