import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { Button, Spinner } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPencil, faTimes, faCheck } from "@fortawesome/free-solid-svg-icons";
import { ToastContainer} from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import Moment from "moment";

import ContextHeader from "../../components/ContextHeader";
import {generalHelper} from "../../helpers/general.helper";
import {fuelCardService} from "../../services/fuelCard.service";

const Details = () => {
  const { id } = useParams();
  const [fuelCard, setfuelCard] = useState({});
  const [pending, setPending] = useState(true);
  const [breadCrumbs, setBreadCrumbs] = useState([]);

  useEffect(() => {
    document.title = "Details Tankkaart";
    fuelCardService.getFuelCard(id)
      .then((response) => {
        setfuelCard(response.data);
        setPending(false);
        setBreadCrumbs([
          { link: "/", name: "Home", active: false },
          { link: "/fuelCard", name: "Tankkaarten", active: false },
          { link: "", name: "Details Tankkaart", active: true },
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
              href={`/fuelCard/edit/${id}`}
            >
              <FontAwesomeIcon icon={faPencil} /> Bewerk
            </Button>
            <Button
              type="button"
              className="btn btn-danger"
              onClick={() => fuelCardService.deleteFuelCard(id)}
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
                    <th>Tankkaart nummer</th>
                    <td>{fuelCard.cardNumber}</td>
                  </tr>
                  <tr>
                    <th>Verval datum</th>
                    <td>
                      {Moment(fuelCard.expirationDate).format("DD-MM-YYYY")}
                    </td>
                  </tr>
                  <tr>
                    <th>Pincode</th>
                    <td>{fuelCard.pincode}</td>
                  </tr>
                  <tr>
                    <th>Actief</th>
                    <td>
                      {fuelCard.isDisabled ? (
                        <FontAwesomeIcon icon={faTimes} />
                      ) : (
                        <FontAwesomeIcon icon={faCheck} />
                      )}
                    </td>
                  </tr>
                  <tr>
                    <th>Brandstof type</th>
                    <td>{fuelCard.fuelType}</td>
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
                        <td>{fuelCard.driverLastName}</td>
                      </tr>
                      <tr>
                        <th>Voornaam</th>
                        <td>{fuelCard.driverFirstName}</td>
                      </tr>
                      <tr>
                        <th>Rijksregisternummer</th>
                        <td>{fuelCard.driverNationalInsuranceNr}</td>
                      </tr>
                      <tr>
                        <th>Geboortedatum</th>
                        <td>
                            {
                                fuelCard.driverdateOfBirth
                                    ? Moment(fuelCard.driverdateOfBirth).format("DD-MM-YYYY")
                                    : null
                            }
                        </td>
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

export default Details;
