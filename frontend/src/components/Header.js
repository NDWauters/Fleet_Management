import logo from "../allphi.png";
import '../styles/Header.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {faArrowRightFromBracket, faCar, faCreditCard, faPerson} from "@fortawesome/free-solid-svg-icons";

const Header = () => {
    return (
        <div className="Header">
            <div className="Top">
                <span className="Welcome">Welkom Andy!</span>
                <button className="btnLogout">
                    <FontAwesomeIcon icon={faArrowRightFromBracket} size="3x"/>
                </button>
            </div>
            <div className="LogoSection">
                <a target={"_blank"} rel="noreferrer" href={"https://www.allphi.eu/"}>
                    <img src={logo} className="App-logo" alt="logo" />
                </a>
            </div>
            <div className="Navigation">
                <div>
                    <FontAwesomeIcon className="Icon" icon={faCar} size="3x"/>
                </div>
                <div>
                    <FontAwesomeIcon className="Icon" icon={faPerson} size="3x"/>
                </div>
                <div>
                    <FontAwesomeIcon className="Icon" icon={faCreditCard} size="3x"/>
                </div>
            </div>
        </div>
    );
}

export default Header;