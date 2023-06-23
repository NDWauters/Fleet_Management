import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faArrowRightFromBracket} from "@fortawesome/free-solid-svg-icons";
import React from "react";
import {userService} from "../services/user.service";
import MySwal from "sweetalert2";
import {Link} from "react-router-dom";

const handleLogout = () => {
    MySwal.fire({
        icon: "question",
        text: "Ben je zeker dat je wilt uitloggen?",
        confirmButtonText: "ja",
        cancelButtonText: "Nee",
        showCancelButton: true,
        confirmButtonColor: "orange",
        reverseButtons: true,
    }).then((result) => {
        if (result.isConfirmed) {
            userService.logout();
        }
    });
};

const LogoutButton = () => (
    <Link className="align-self-center" to={""} onClick={handleLogout}>
        <FontAwesomeIcon className="Icon" icon={faArrowRightFromBracket} size={"2x"}/>
    </Link>
);

export default LogoutButton;
