import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {Button, Spinner} from "react-bootstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPencil, faTimes} from "@fortawesome/free-solid-svg-icons";
import {ToastContainer} from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import ContextHeader from "../../components/ContextHeader";
import {vehicleService} from "../../services/vehicle.service";
import {generalHelper} from "../../helpers/general.helper";

const VehicleDetails = () => {

    const {id} = useParams();
    const [vehicle, setVehicle] = useState({});
    const [pending, setPending] = useState(true);
    const [breadCrumbs, setBreadCrumbs] = useState([]);

    useEffect(() => {
        document.title = "Details Voertuig";
        vehicleService.getVehicle(id)
            .then((response) => {
                setVehicle(response.data);
                setPending(false);
                setBreadCrumbs([
                    {link: "/", name: "Home", active: false},
                    {link: "/vehicle", name: "Voertuigen", active: false},
                    {link: "", name: "Details Voertuig", active: true},
                ]);
            })
            .catch((err) => generalHelper.handleError(err));
    }, []);

    return (
        <main>
            <ContextHeader breadCrumbs={breadCrumbs}/>
            {pending ? (
                <div className="text-center mt-5">
                    <Spinner animation="border"/>
                </div>
            ) : (
                <div>
                    <div className="btnActions d-flex flex-row justify-content-end m-3">
                        <Button
                            type="button"
                            className="btn btn-warning me-2"
                            href={`/vehicle/edit/${id}`}
                        >
                            <FontAwesomeIcon icon={faPencil}/> Bewerk
                        </Button>
                        <Button
                            type="button"
                            className="btn btn-danger"
                            onClick={() => vehicleService.deleteVehicle(id)}
                        >
                            <FontAwesomeIcon icon={faTimes}/> Verwijder
                        </Button>
                    </div>
                    <div className="row m-3">
                        <div className="col-md-4 border">
                            <h5 className="mt-2">Algemeen</h5>
                            <table className="table">
                                <tbody>
                                <tr>
                                    <th>Merk</th>
                                    <td>{vehicle.brand}</td>
                                </tr>
                                <tr>
                                    <th>Model</th>
                                    <td>{vehicle.model}</td>
                                </tr>
                                <tr>
                                    <th>Brandstoftype</th>
                                    <td>{vehicle.fuelType}</td>
                                </tr>
                                <tr>
                                    <th>Chassisnummer</th>
                                    <td>{vehicle.vin}</td>
                                </tr>
                                <tr>
                                    <th>Nummerplaat</th>
                                    <td>{vehicle.licensePlate}</td>
                                </tr>
                                </tbody>
                            </table>
                        </div>
                        <div className="col-md-4 border">
                            <h5 className="mt-2">Visueel</h5>
                            <table className="table">
                                <tbody>
                                <tr>
                                    <th>Voertuigtype</th>
                                    <td>{vehicle.vehicleType}</td>
                                </tr>
                                <tr>
                                    <th>Kleur</th>
                                    <td>{vehicle.color}</td>
                                </tr>
                                <tr>
                                    <th>Aantal deuren</th>
                                    <td>{vehicle.amountDoors}</td>
                                </tr>
                                </tbody>
                            </table>
                        </div>
                        <div className="col-md-4 border">
                            <h5 className="mt-2">Bestuurder</h5>
                            <table className="table">
                                <tbody>
                                <tr>
                                    <th>Familienaam</th>
                                    <td>{vehicle.driverLastName}</td>
                                </tr>
                                <tr>
                                    <th>Voornaam</th>
                                    <td>{vehicle.driverFirstName}</td>
                                </tr>
                                <tr>
                                    <th>Tankkaart</th>
                                    <td>{vehicle.fuelCard}</td>
                                </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <ToastContainer
                        position="bottom-right"
                        autoClose={5000}
                        hideProgressBar={false}
                        newestOnTop={false}
                        closeOnClick
                        rtl={false}
                        pauseOnFocusLoss
                        draggable
                        pauseOnHover
                    />
                </div>
            )}
        </main>
    );
};

export default VehicleDetails;
