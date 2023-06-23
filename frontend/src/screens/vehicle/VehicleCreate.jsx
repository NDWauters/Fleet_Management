import React, { useEffect, useState } from 'react';
import { Formik, Form } from 'formik';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSave } from '@fortawesome/free-solid-svg-icons';
import { useNavigate } from 'react-router-dom';
import { Spinner } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';

import ContextHeader from '../../components/ContextHeader';
import CreateValidationScheme from '../../components/vehicle/form/createValidationScheme';
import SelectListField from '../../components/forms/SelectListField';
import TextBoxField from '../../components/forms/TextBoxField';
import {vehicleService} from "../../services/vehicle.service";
import {generalHelper} from "../../helpers/general.helper";

const VehicleCreate = () => {

  const [selectLists, setSelectLists] = useState([]);
  const [isPending, setIsPending] = useState(true);
  const [breadCrumbs, setBreadCrumbs] = useState([]);

  const navigate = useNavigate();

  useEffect(() => {
    document.title = 'Aanmaak Voertuig';
    vehicleService.getSelectListsVehicle()
      .then((response) => {
        setSelectLists(response.data);
        setIsPending(false);
        setBreadCrumbs([
          { link: '/', name: 'Home', active: false },
          { link: '/vehicle', name: 'Voertuigen', active: false },
          { link: '', name: 'Nieuw', active: true },
        ]);
      })
      .catch((err) => {
        generalHelper.handleError(err);
      });
  }, []);

  const onSubmit = (values) => {

      values.driverID = generalHelper.convertSelectListValue(values.driverID);

    vehicleService.createVehicle(values)
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
                brandID: null,
                vehicleTypeID: null,
                driverID: null,
                fuelTypeID: null,
                color: '',
                amountDoors: 1,
                model: '',
                licensePlate: '',
                vin: '',
              }}
              onSubmit={(values) => onSubmit(values)}
              validationSchema={CreateValidationScheme}
            >
              {({ errors }) => (
                <Form>
                  <div className="row">
                    <div className="col-md-3"></div>
                    <div className="col-md-6">
                      <div className="row">
                        <div className="col-md-6">
                          <SelectListField
                            name="brandID"
                            label="Merk"
                            required={true}
                            selectListItems={selectLists.brandSelectList}
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
                            selectListItems={selectLists.vehicleTypeSelectList}
                          />
                        </div>
                        <div className="col-md-6">
                          <SelectListField
                            name="fuelTypeID"
                            label="Brandstoftype"
                            required={true}
                            selectListItems={selectLists.fuelTypeSelectList}
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
                            selectListItems={selectLists.driverSelectList}
                          />
                        </div>
                      </div>
                      <div className="row">
                        <div className="col-md-12">
                          <hr />
                          <button
                            className="btn btn-success float-end"
                            type="submit"
                          >
                            <FontAwesomeIcon icon={faSave} /> Opslaan
                          </button>
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

export default VehicleCreate;
