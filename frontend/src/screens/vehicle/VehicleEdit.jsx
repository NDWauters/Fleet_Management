import React, { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { toast, ToastContainer } from 'react-toastify';
import { Spinner } from 'react-bootstrap';
import { Form, Formik } from 'formik';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSave, faTimes } from '@fortawesome/free-solid-svg-icons';

import ContextHeader from '../../components/ContextHeader';
import CreateValidationScheme from '../../components/vehicle/form/createValidationScheme';
import SelectListField from '../../components/forms/SelectListField';
import TextBoxField from '../../components/forms/TextBoxField';
import HiddenField from '../../components/forms/HiddenField';
import {vehicleService} from "../../services/vehicle.service";
import {generalHelper} from "../../helpers/general.helper";

const VehicleEdit = () => {
  const { id } = useParams();
  const [data, setData] = useState([]);
  const [isPending, setIsPrending] = useState(true);
  const [breadCrumbs, setBreadCrumbs] = useState([]);

  const navigate = useNavigate();

  useEffect(() => {
    document.title = 'Bewerk Voertuig';
    vehicleService.getSelectListsVehicle(id)
      .then((response) => {
        setData(response.data);
        setIsPrending(false);
        setBreadCrumbs([
          { link: '/', name: 'Home', active: false },
          { link: '/vehicle', name: 'Voertuigen', active: false },
          { link: '/vehicle/details/1', name: 'Details', active: false },
          { link: '', name: 'Bewerk', active: true },
        ]);
      })
      .catch((err) => {
          generalHelper.handleError(err);
      });
  }, []);

  const onSubmit = (values) => {

      values.driverID = generalHelper.convertSelectListValue(values.driverID);

      vehicleService.editVehicle(values)
      .then((response) => {
          const id = response.data.id;
          if (id > 0){
              navigate(`/vehicle/details/${response.data.id}`, { replace: true });
          }else {
              if (id == -1){
                  toast('Chassisnummer is al in gebruik.', { type: 'error' })
              }else {
                  toast('Nummerplaat is al in gebruik.', { type: 'error' })
              }
          }
      })
      .catch((err) => {
          generalHelper.handleError(err);
      });
  };

  return (
    <main>
      <ContextHeader breadCrumbs={breadCrumbs} />
      <div className="row m-3">
        <div className="col-md-12">
          {isPending ? (
            <div className="text-center mt-5">
              <Spinner animation={'border'} />
            </div>
          ) : (
            <Formik
              initialValues={{
                vehicleID: data.vehicleID,
                brandID: data.brandID,
                vehicleTypeID: data.vehicleTypeID,
                driverID: data.driverID,
                fuelTypeID: data.fuelTypeID,
                color: data.color,
                amountDoors: data.amountDoors,
                model: data.model,
                licensePlate: data.licensePlate,
                vin: data.vin,
              }}
              onSubmit={(values) => onSubmit(values)}
              validationSchema={CreateValidationScheme}
            >
              {({ errors }) => (
                <Form>
                  <HiddenField name="vehicleID" />
                  <div className="row">
                    <div className="col-md-3"></div>
                    <div className="col-md-6">
                      <div className="row">
                        <div className="col-md-6">
                          <SelectListField
                            name="brandID"
                            label="Merk"
                            required={true}
                            selectListItems={data.brandSelectList}
                          />
                        </div>
                        <div className="col-md-6">
                          <TextBoxField
                            name="model"
                            label="Model"
                            required={true}
                          />
                        </div>
                      </div>
                      <div className="row">
                        <div className="col-md-6">
                          <SelectListField
                            name="vehicleTypeID"
                            label="VoertuigType"
                            required={true}
                            selectListItems={data.vehicleTypeSelectList}
                          />
                        </div>
                        <div className="col-md-6">
                          <SelectListField
                            name="fuelTypeID"
                            label="Brandstoftype"
                            required={true}
                            selectListItems={data.fuelTypeSelectList}
                          />
                        </div>
                      </div>
                      <div className="row">
                        <div className="col-md-6">
                          <TextBoxField
                            name="licensePlate"
                            label="Nummerplaat"
                            required={true}
                          />
                        </div>
                        <div className="col-md-6">
                          <TextBoxField
                            name="vin"
                            label="Chassisnummer"
                            required={true}
                          />
                        </div>
                      </div>
                      <div className="row">
                        <div className="col-md-6">
                          <TextBoxField name="color" label="Kleur" />
                        </div>
                        <div className="col-md-6">
                          <TextBoxField
                            name="amountDoors"
                            label="Aantal deuren"
                          />
                        </div>
                      </div>
                      <div className="row">
                        <div className="col-md-6">
                          <SelectListField
                            name="driverID"
                            label="Bestuurder"
                            selectListItems={data.driverSelectList}
                          />
                        </div>
                      </div>
                      <div className="row">
                        <div className="col-md-12">
                          <hr />
                          <div className="d-flex flex-row justify-content-end">
                            <Link
                              className="btn btn-light"
                              type="button"
                              to={`/vehicle/details/${id}`}
                            >
                              <FontAwesomeIcon icon={faTimes} /> Annuleer
                            </Link>
                            <button
                              className="btn btn-success me-2"
                              type="submit"
                            >
                              <FontAwesomeIcon icon={faSave} /> Opslaan
                            </button>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div className="col-md-3"></div>
                  </div>
                </Form>
              )}
            </Formik>
          )}
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
    </main>
  );
};

export default VehicleEdit;
