import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import {Button, Spinner} from "react-bootstrap";
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import ContextHeader from "../../components/ContextHeader";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPencil, faTimes } from "@fortawesome/free-solid-svg-icons";
import Moment from "moment";
import {driverService} from "../../services/driver.service";
import {generalHelper} from "../../helpers/general.helper";

const DriverDetails = () => {
  const { id } = useParams();
  const [driver, setDriver] = useState({});
  const [pending, setPending] = useState(true);
  const [breadCrumbs, setBreadCrumbs] = useState([]);

  useEffect(() => {
    document.title = "Details Bestuurder";
    driverService.getDriver(id)
      .then((response) => {
        setDriver(response.data);
        setPending(false);
        setBreadCrumbs([
          { link: "/", name: "Home", active: false },
          { link: "/driver", name: "Bestuurders", active: false },
          { link: "", name: "Details Bestuurder", active: true },
        ]);
      })
      .catch((err) => generalHelper.handleError(err));
  }, []);

    return (
        <main>
            <ContextHeader breadCrumbs={breadCrumbs} />
            {pending ? (
                <div className="text-center mt-5">
                    <Spinner animation="border" />
                </div>
            ) : (
                <div>
                    <div className="btnActions d-flex flex-row justify-content-end m-3">
                        <Button
                            type="button"
                            className="btn btn-warning me-2"
                            href={`/driver/edit/${id}`}
                        >
                            <FontAwesomeIcon icon={faPencil} /> Bewerk
                        </Button>
                        <Button
                            type="button"
                            className="btn btn-danger"
                            onClick={() => driverService.deleteDriver(id)}
                        >
                            <FontAwesomeIcon icon={faTimes} /> Verwijder
                        </Button>
                    </div>
                    <div className="row m-3">

                        <div className="col-md-4 border">
                            <h5 className="mt-2">Algemeen</h5>
                            <table className="table">
                                <tbody>
                                <tr>
                                    <th>Naam</th>
                                    <td>{driver.name}</td>
                                </tr>
                                <tr>
                                    <th>Geboortedatum</th>
                                    <td>{Moment(driver.dateOfBirth).format("DD-MM-YYYY")}</td>
                                </tr>
                                <tr>
                                    <th>Rijksregisternummer</th>
                                    <td>{driver.nationalInsuranceNr}</td>
                                </tr>
                                <tr>
                                    <th>Rijbewijzen</th>
                                    <td>{driver.driverLicenseType}</td>
                                </tr>
                                <tr>
                                    <th>Tankkaartnummer</th>
                                    <td>{driver.cardNumber}</td>
                                </tr>
                                <tr>
                                    <th>Tankkaart vervaldatum</th>
                                    <td>
                                        {
                                            driver.expirationDate
                                                ? Moment(driver.expirationDate).format("DD-MM-YYYY")
                                                : null
                                        }
                                    </td>
                                </tr>
                                </tbody>
                            </table>
                        </div>

            <div className="col-md-4 border">
              <h5 className="mt-2">Adres</h5>
              <table className="table">
                <tbody>
                  <tr>
                    <th>Straat</th>
                    <td>{driver.street}</td>
                  </tr>
                  <tr>
                    <th>Nummer</th>
                    <td>{driver.number}</td>
                  </tr>
                  <tr>
                    <th>Postcode</th>
                    <td>{driver.zipcode}</td>
                  </tr>
                  <tr>
                    <th>Plaats</th>
                    <td>{driver.place}</td>
                  </tr>
                </tbody>
              </table>
            </div>

            <div className="col-md-4 border">
              <h5 className="mt-2">Voertuig</h5>
              <table className="table">
                <tbody>
                  <tr>
                    <th>Merk</th>
                    <td>{driver.brand}</td>
                  </tr>
                  <tr>
                    <th>Model</th>
                    <td>{driver.model}</td>
                  </tr>
                  <tr>
                    <th>Nummerplaat</th>
                    <td>{driver.licensePlate}</td>
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

export default DriverDetails;
