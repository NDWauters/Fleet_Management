import React from 'react';
import LoginPage from "../screens/account/LoginPage";
import HomePage from "../screens/home/HomePage";

export const PrivateRoute = () => {

    return (
        localStorage.getItem('user')
                ? <HomePage/>
                : <LoginPage/>
    );
}