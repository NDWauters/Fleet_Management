import React, { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { toast, ToastContainer } from 'react-toastify';
import { Spinner } from 'react-bootstrap';
import { Form, Formik, Field } from 'formik';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSave, faTimes } from '@fortawesome/free-solid-svg-icons';

import ContextHeader from '../../components/ContextHeader';
import CreateValidationScheme from '../../components/fuelcard/form/CreateValidationScheme';
import SelectListField from '../../components/forms/SelectListField';
import TextBoxField from '../../components/forms/TextBoxField';
import HiddenField from '../../components/forms/HiddenField';
import SelectField from '../../components/CustomSelect';
import CheckBoxField from '../../components/forms/CheckboxField';
import {generalHelper} from "../../helpers/general.helper";
import {fuelCardService} from "../../services/fuelCard.service";
import DatePickerField from "../../components/forms/DatePickerField";

const Edit = () => {
  const { id } = useParams();
  const [data, setData] = useState([]);
  const [isPending, setIsPrending] = useState(true);
  const [breadCrumbs, setBreadCrumbs] = useState([]);
  const [startDate, setStartDate] = useState(new Date());

  const navigate = useNavigate();

  useEffect(() => {
    document.title = 'Bewerk Tankkaart';
    fuelCardService.getSelectListsFuelCard(id)
      .then((response) => {
        setData(response.data);
        setIsPrending(false);
        setBreadCrumbs([
          { link: '/', name: 'Home', active: false },
          { link: '/fuelCard', name: 'Tankkaarten', active: false },
          { link: '/FuelCard/details/1', name: 'Details', active: false },
          { link: '', name: 'Bewerk', active: true },
        ]);
      })
      .catch((err) => {
        generalHelper.handleError(err);
      });
  }, []);

  const onSubmit = (values) => {
    values.fuelTypeID = values.fuelTypeID.map((e) => e.value);
    values.driverID = generalHelper.convertSelectListValue(values.driverID);

    fuelCardService.editFuelCard(values)
      .then((response) => {
        if(response.data.id != 0){
        navigate(`/fuelCard/details/${response.data.id}`, { replace: true });
      }
      else{
        toast('Het kaartnummer is reeds in gebruik.');
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
                fuelCardID: data.fuelCardID,
                cardNumber: data.cardNumber,
                expirationDate: new Date(data.expirationDate),
                pincode: data.pincode,
                fuelTypeID: generalHelper.convertMultiSelectValues(
                  data.fuelTypeID,
                  data.fuelTypeSelectList
                ),
                driverID: data.driverID ?? null,
                isDisabled: data.isDisabled,
              }}
              onSubmit={(values) => onSubmit(values)}
              validationSchema={CreateValidationScheme}
            >
              {({ errors, values }) => (
                <Form>
                  <HiddenField name="fuelCardID" />
                  <div className="row">
                    <div className="col-md-3"></div>
                    <div className="col-md-6">
                      <div className="row">
                        <div className="col-md-6">
                          <TextBoxField
                            name="cardNumber"
                            label="Kaart nummer"
                            required={true}
                          />
                        </div>
                        <div className="col-md-6">
                          <Field
                            component={SelectField}
                            name="fuelTypeID"
                            options={data.fuelTypeSelectList}
                            label="Brandstof type"
                            required={true}
                          />
                        </div>
                      </div>
                    </div>
                  </div>
                  <div className="row">
                    <div className="col-md-3"></div>
                    <div className="col-md-6">
                      <div className="row">
                        <div className="col-md-6">
                        <DatePickerField
                              name="expirationDate"
                              label="Vervaldatum"
                              required={true}
                              selected={startDate}
                              onChange={(date) => setStartDate(date)}
                              value={startDate}
                              tooltip={"dd-MM-yyyy"}
                          />
                        </div>
                        <div className="col-md-6">
                          <TextBoxField name="pincode" label="Pincode" />
                        </div>
                      </div>
                    </div>
                  </div>
                  <div className="row">
                    <div className="col-md-3"></div>
                    <div className="col-md-6">
                      <div className="row">
                        <div className="col-md-6">
                          <SelectListField
                            name="driverID"
                            label="Bestuurder"
                            selectListItems={data.driverSelectList}
                          />
                        </div>
                        <div className="col-md-1">
                          <CheckBoxField
                            name="isDisabled"
                            label="Geblokkeerd"
                            required={true}
                            value={values.isDisabled}
                          />
                        </div>
                      </div>
                    </div>
                  </div>
                  <div className="row">
                    <div className="row">
                      <div className="col-md-12">
                        <hr />
                        <div className="d-flex flex-row justify-content-end">
                          <Link
                            className="btn btn-light"
                            type="button"
                            to={`/fuelCard/details/${id}`}
                          >
                            <FontAwesomeIcon icon={faTimes} /> Annuleer
                          </Link>
                          <button
                            className="btn btn-success me-2"
                            type="submit"
                            onClick={(values) => console.log(values)}
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

export default Edit;
