import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes, faMagic } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import { Dropdown } from "react-bootstrap";
import {fuelCardService} from "../../services/fuelCard.service";
import moment from "moment";

const FuelCardColumns = [
  {
    name: "Tankkaart nummer",
    selector: (row) => row.cardNumber,
  },
  {
    name: "Vervaldatum",
    selector: (row) => moment(new Date(row.expirationDate)).format('DD-MM-YYYY'),
  },
  {
    name: "Pincode",
    selector: (row) => row.pincode,
  },
  {
    name: "Bestuurder",
    cell: (row) => {
      return row.driver ? (
        <Link
          className="btn btn-secondary btn-sm"
          type="button"
          to={`/driver/details/${row.driverID}`}
        >
          {row.driver}
        </Link>
      ) : (
        ""
      );
    },
  },
  {
    name: "Actief",
    cell: (row) =>
      row.isdisabled ? (
        <FontAwesomeIcon icon={faTimes} />
      ) : (
        <FontAwesomeIcon icon={faCheck} />
      ),
  },
  {
    name: "Brandstof",
    selector: (row) => row.fuelType,
  },
  {
    name: "Acties",
    button: true,
    cell: (row) => (
      <Dropdown>
        <Dropdown.Toggle variant="default">
          <FontAwesomeIcon icon={faMagic} /> Acties
        </Dropdown.Toggle>
        <Dropdown.Menu className="dropdown-menu">
          <Dropdown.Item href={`/fuelCard/details/${row.fuelCardID}`}>
            Details
          </Dropdown.Item>

          <Dropdown.Item href={`/fuelCard/edit/${row.fuelCardID}`}>
            Bewerk
          </Dropdown.Item>

          <Dropdown.Item onClick={() => fuelCardService.deleteFuelCard(row.fuelCardID)}>
            Verwijder
          </Dropdown.Item>
        </Dropdown.Menu>
      </Dropdown>
    ),
  },
];

export default FuelCardColumns;
