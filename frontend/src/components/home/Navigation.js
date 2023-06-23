import React from "react";
import "../../styles/Navigation.css";
import { Link } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faCar,
    faCreditCard,
    faPerson,
} from "@fortawesome/free-solid-svg-icons";

const Navigation = ({ size }) => {

    const url = window.location.href;

    return (
        <div className="row">
            <div className="d-flex flex-row justify-content-center mt-3 mb-3">
                <Link to={"/vehicle"}>
                    <FontAwesomeIcon className={url.includes("/vehicle") ? "Icon activeIcon" : "Icon"} icon={faCar} size={size}/>
                </Link>
                <Link to={"/driver"}>
                    <FontAwesomeIcon className={url.includes("/driver") ? "Icon activeIcon" : "Icon"} icon={faPerson} size={size}/>
                </Link>
                <Link to={"/fuelCard"}>
                    <FontAwesomeIcon className={url.includes("/fuelCard") ? "Icon activeIcon" : "Icon"} icon={faCreditCard} size={size}/>
                </Link>
            </div>
        </div>
    );
}

export default Navigation;
