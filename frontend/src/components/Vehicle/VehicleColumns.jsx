import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMagic } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import { Dropdown } from "react-bootstrap";
import {vehicleService} from "../../services/vehicle.service";

const VehicleColumns = [
  {
    name: "Merk",
    selector: (row) => row.brand,
  },
  {
    name: "Model",
    selector: (row) => row.model,
  },
  {
    name: "Nummerplaat",
    selector: (row) => row.licensePlate,
  },
  {
    name: "Chassisnummer",
    selector: (row) => row.vin,
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
    name: "Acties",
    button: true,
    cell: (row) => (
      <Dropdown>
        <Dropdown.Toggle variant="default">
          <FontAwesomeIcon icon={faMagic} /> Acties
        </Dropdown.Toggle>
        <Dropdown.Menu className="dropdown-menu">
          <Dropdown.Item href={`/vehicle/details/${row.vehicleID}`}>
            Details
          </Dropdown.Item>
          <Dropdown.Item href={`/vehicle/edit/${row.vehicleID}`}>
            Bewerk
          </Dropdown.Item>
          <Dropdown.Item onClick={() => vehicleService.deleteVehicle(row.vehicleID)}>
            Verwijder
          </Dropdown.Item>
        </Dropdown.Menu>
      </Dropdown>
    ),
  },
];

export default VehicleColumns;
