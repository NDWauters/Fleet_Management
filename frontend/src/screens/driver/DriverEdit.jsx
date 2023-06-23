import React, { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { toast, ToastContainer } from 'react-toastify';
import ContextHeader from '../../components/ContextHeader';
import { Spinner } from 'react-bootstrap';
import { Formik, Form, Field } from 'formik';
import CreateValidationScheme from '../../components/driver/form/CreateValidationScheme';
import SelectListField from '../../components/forms/SelectListField';
import TextBoxField from '../../components/forms/TextBoxField';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSave, faTimes } from '@fortawesome/free-solid-svg-icons';
import SelectField from '../../components/CustomSelect';
import HiddenField from '../../components/forms/HiddenField';
import DatePickerField from "../../components/forms/DatePickerField";
import {driverService} from "../../services/driver.service";
import {generalHelper} from "../../helpers/general.helper";

const DriverEdit = () => {
    const { id } = useParams();
    const [driver, setDriver] = useState([]);
    const [isPending, setIsPending] = useState(true);
    const [breadCrumbs, setBreadCrumbs] = useState([]);
    const [startDate, setStartDate] = useState(new Date());

  const navigate = useNavigate();

  useEffect(() => {
    document.title = 'Bewerk Bestuurder';
    driverService.getSelectListsDriver(id)
      .then((response) => {
        setDriver(response.data);
        setIsPending(false);
        setBreadCrumbs([
          { link: '/', name: 'Home', active: false },
          { link: '/Driver', name: 'Bestuurders', active: false },
          { link: '/Driver/Details/1', name: 'Details', active: false },
          { link: '', name: 'Bewerk', active: true },
        ]);
        setStartDate(new Date(response.data.dateOfBirth));
      })
      .catch((err) => {
          generalHelper.handleError(err);
      });
  }, []);

  const onSubmit = (values) => {

      values.driverLicenseTypeID = values.driverLicenseTypeID.map(e => e.value);
      values.vehicleID = generalHelper.convertSelectListValue(values.vehicleID);
      values.fuelCardID = generalHelper.convertSelectListValue(values.fuelCardID);

    driverService.editDriver(values)
      .then((response) => {
          if(response.data.id != 0){
              navigate(`/driver/details/${response.data.id}`, { replace: true });
          }
          else{
              toast('Het rijksregisternummer bestaat al');
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
                driverID: driver.driverID,
                lastName: driver.lastName,
                firstName: driver.firstName,
                dateOfBirth: new Date(driver.dateOfBirth),
                nationalInsuranceNr: driver.nationalInsuranceNr,
                address: {
                  street: driver.address.street,
                  number: driver.address.number,
                  zipcode: driver.address.zipcode,
                  place: driver.address.place,
                },
                driverLicenseTypeID: generalHelper.convertMultiSelectValues(
                  driver.driverLicenseTypeID,
                  driver.driverLicenseTypeSelectList
                ),
                vehicleID: driver.vehicleID,
                fuelCardID: driver.fuelCardID,
                cardNumber: driver.cardNumber,
              }}
              onSubmit={(values) => onSubmit(values)}
              validationSchema={CreateValidationScheme}
            >
              {({ errors }) => (
                <Form>
                  <HiddenField name="driverID" />
                  <div className="row">
                    <div className="col-md-3"></div>
                    <div className="col-md-6">
                      <div className="row">
                        <div className="col-md-6">
                          <TextBoxField
                            name="lastName"
                            label="Familienaam"
                            required={true}
                          />
                        </div>
                        <div className="col-md-6">
                          <TextBoxField
                            name="firstName"
                            label="Voornaam"
                            required={true}
                          />
                        </div>
                      </div>

                      <div className="row">
                        <div className="col-md-6">
                          <DatePickerField
                              name="dateOfBirth"
                              label="Geboortedatum"
                              required={true}
                              selected={startDate}
                              onChange={(date) => setStartDate(date)}
                              value={startDate}
                              tooltip={"dd-MM-yyyy"}
                          />
                        </div>
                        <div className="col-md-6">
                          <TextBoxField
                            name="nationalInsuranceNr"
                            label="Rijksregisternummer"
                            required={true}
                            tooltip={"example: 02021518897′"}
                          />
                        </div>
                      </div>

                      <div className="row">
                        <div className="col-md-6">
                          <TextBoxField
                            name="address.street"
                            label="Straat"
                            required={true}
                          />
                        </div>
                        <div className="col-md-6">
                          <TextBoxField
                            name="address.number"
                            label="Nummer"
                            required={true}
                          />
                        </div>
                      </div>

                      <div className="row">
                        <div className="col-md-6">
                          <TextBoxField
                            name="address.zipcode"
                            label="Postcode"
                            required={true}
                          />
                        </div>
                        <div className="col-md-6">
                          <TextBoxField
                            name="address.place"
                            label="Plaats"
                            required={true}
                          />
                        </div>
                      </div>

                      <div className="row">
                        <div className="col-md-6">
                          <Field
                            component={SelectField}
                            name="driverLicenseTypeID"
                            options={driver.driverLicenseTypeSelectList}
                            label="Rijbewijstype"
                            required={true}
                          />
                        </div>
                      </div>

                      <div className="row">
                        <div className="col-md-6">
                          <SelectListField
                            name="vehicleID"
                            label="Voertuig"
                            required={false}
                            selectListItems={driver.vehicleSelectList}
                          />
                        </div>
                        <div className="col-md-6">
                          <SelectListField
                            name="fuelCardID"
                            label="Tankkaart"
                            required={false}
                            selectListItems={driver.fuelCardSelectList}
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
                              to={`/Driver/Details/${id}`}
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

export default DriverEdit;
