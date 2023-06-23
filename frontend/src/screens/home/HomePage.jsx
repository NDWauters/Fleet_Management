import React from 'react';
import Header from "../../components/home/Header";
import Logo from "../../components/Logo";
import Navigation from "../../components/home/Navigation";
import Footer from "../../components/home/Footer";
import "../../styles/App.css"

function HomePage() {
    return (
        <div className="App">
            <Header />
            <Logo />
            <Navigation size="3x" />
            <Footer />
        </div>
    );
}

export default HomePage;
