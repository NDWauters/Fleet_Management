import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMagic } from "@fortawesome/free-solid-svg-icons";
import {Link} from "react-router-dom";
import {Dropdown} from "react-bootstrap";
import {driverService} from "../../services/driver.service";

const DriverColumns = [
    {
        name: 'Naam',
        selector: row => row.name,
    },
    {
        name: 'Plaats',
        selector: row => row.place,
    },
    {
        name: 'Voertuig',
        cell: (row) => {
            return(
                row.licensePlate
                    ?
                    <Link className="btn btn-secondary btn-sm" type="button" to={`/vehicle/details/${row.vehicleID}`}>
                        {row.licensePlate}
                    </Link>
                    :
                    ""
            );
        },
    },
    {
        name: 'Tankkaart',
        cell: (row) => {
            return(
                row.cardNumber
                    ?
                    <Link className="btn btn-secondary btn-sm" type="button" to={`/fuelCard/details/${row.fuelCardID}`}>
                        {row.cardNumber}
                    </Link>
                    :
                    ""
            );
        },
    },
    {
        name: 'Acties',
        button: true,
        cell: (row) => (
            <Dropdown>
                <Dropdown.Toggle variant="default">
                    <FontAwesomeIcon icon={faMagic}/> Acties
                </Dropdown.Toggle>
                <Dropdown.Menu className="dropdown-menu">
                    <Dropdown.Item href={`/driver/details/${row.driverID}`}>Details</Dropdown.Item>
                    <Dropdown.Item href={`/driver/edit/${row.driverID}`}>Bewerk</Dropdown.Item>
                    <Dropdown.Item onClick={() => driverService.deleteDriver(row.driverID)}>Verwijder</Dropdown.Item>
                </Dropdown.Menu>
            </Dropdown>
        ),
    }
];

export default DriverColumns;