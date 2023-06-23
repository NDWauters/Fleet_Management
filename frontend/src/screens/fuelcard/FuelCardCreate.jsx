import React, { useEffect, useState } from 'react';
import { Formik, Form, Field } from 'formik';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSave } from '@fortawesome/free-solid-svg-icons';
import { Spinner } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';

import SelectField from '../../components/CustomSelect';
import ContextHeader from '../../components/ContextHeader';
import CreateValidationScheme from '../../components/fuelcard/form/CreateValidationScheme';
import SelectListField from '../../components/forms/SelectListField';
import TextBoxField from '../../components/forms/TextBoxField';
import CheckboxField from '../../components/forms/CheckboxField';
import {generalHelper} from "../../helpers/general.helper";
import {fuelCardService} from "../../services/fuelCard.service";
import DatePickerField from "../../components/forms/DatePickerField";

const Create = () => {
  const [selectLists, setSelectLists] = useState([]);
  const [isPending, setIsPrending] = useState(true);
  const [breadCrumbs, setBreadCrumbs] = useState([]);
  const [startDate, setStartDate] = useState(new Date());

  const navigate = useNavigate();

  useEffect(() => {
    document.title = 'Aanmaak Tankkaart';
    fuelCardService.getSelectListsFuelCard()
      .then((response) => {
        setSelectLists(response.data);
        setIsPrending(false);
        setBreadCrumbs([
          { link: '/', name: 'Home', active: false },
          { link: '/fuelCard', name: 'Tankkaarten', active: false },
          { link: '', name: 'Nieuw', active: true },
        ]);
      })
      .catch((err) => {
          generalHelper.handleError(err);
      });
  }, []);

  const onSubmit = (values) => {
    values.fuelTypeID = values.fuelTypeID.map((e) => e.value);
    values.driverID = generalHelper.convertSelectListValue(values.driverID);

    fuelCardService.createFuelCard(values)
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
}


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
                driverID: null,
                fuelTypeID: [],
                cardNumber: '',
                expirationDate: '',
                pincode: '',
                isDisabled: false,
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
                          <TextBoxField
                            name="cardNumber"
                            label="Tankkaart nummer"
                            required={true}
                          />
                        </div>
                        <div className="col-md-6">
                          <Field
                            component={SelectField}
                            name="fuelTypeID"
                            options={selectLists.fuelTypeSelectList}
                            label="Brandstof type"
                            required={true}
                          />
                        </div>
                      </div>
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
                      <div className="row">
                        <div className="col-md-6">
                          <SelectListField
                            name="driverID"
                            label="Bestuurder"
                            selectListItems={selectLists.driverSelectList}
                            required={false}
                          />
                        </div>
                        <div className="col-md-1">
                          <CheckboxField
                            name="isDisabled"
                            required={true}
                            label="Geblokkeerd"
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

export default Create;
