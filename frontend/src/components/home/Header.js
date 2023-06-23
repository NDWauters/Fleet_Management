import React from 'react';
import {useSelector} from "react-redux";
import LogoutButton from "../LogoutButton";

const Header = () => {

    const user = useSelector(state => state.user.user)

    return (
        <div className="d-flex flex-row justify-content-between">
            <p className="ms-2">Welkom {user.firstName}!</p>
            <LogoutButton />
        </div>
    );
}

export default Header;