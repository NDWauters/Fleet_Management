import React, {useEffect, useState} from 'react';
import {faSave} from '@fortawesome/free-solid-svg-icons';
import {Spinner} from 'react-bootstrap';
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {useNavigate} from 'react-router-dom';
import {ToastContainer, toast} from 'react-toastify';
import {Formik, Form, Field} from 'formik';
import "react-datepicker/dist/react-datepicker.css";

import ContextHeader from '../../components/ContextHeader';
import CreateValidationScheme from '../../components/driver/form/CreateValidationScheme';
import SelectListField from "../../components/forms/SelectListField";
import TextBoxField from "../../components/forms/TextBoxField";
import SelectField from "../../components/CustomSelect";
import DatePickerField from "../../components/forms/DatePickerField";
import {driverService} from "../../services/driver.service";
import {generalHelper} from "../../helpers/general.helper";

const DriverCreate = () => {

    const [selectLists, setSelectLists] = useState([]);
    const [isPending, setIsPending] = useState(true);
    const [breadCrumbs, setBreadCrumbs] = useState([]);
    const [startDate, setStartDate] = useState(new Date());

    const navigate = useNavigate();

    useEffect(() => {
        document.title = "Aanmaak Bestuurder";
        driverService.getSelectListsDriver()
            .then((response) => {
                setSelectLists(response.data);
                setIsPending(false);
                setBreadCrumbs([
                    {link: "/", name: "Home", active: false},
                    {link: "/Driver", name: "Bestuurders", active: false},
                    {link: "", name: "Nieuw", active: true},
                ]);
            })
            .catch((err) => {
                generalHelper.handleError(err);
            });
    }, []);

    const onSubmit = (values) => {

        values.driverLicenseTypeID = values.driverLicenseTypeID.map(e => e.value);
        values.vehicleID = generalHelper.convertSelectListValue(values.vehicleID);
        values.fuelCardID = generalHelper.convertSelectListValue(values.fuelCardID);

        driverService.createDriver(values)
            .then((response) => {
                if(response.data.id != 0){
                    navigate(`/driver/details/${response.data.id}`, {replace: true});
                }
                else{
                    toast('Het rijksregisternummer bestaat al');
                }
            })
            .catch((err) => {
                generalHelper.handleError(err);
            });
    }

    return (
        <main>
            <ContextHeader breadCrumbs={breadCrumbs}/>
            <div className="row m-3">
                <div className="col-md-12">
                    {isPending ?
                        <div className="text-center mt-5">
                            <Spinner animation={'border'}/>
                        </div>
                     :
                        <Formik
                            initialValues={{
                                lastName: '',
                                firstName: '',
                                dateOfBirth: '',
                                nationalInsuranceNr: '',
                                address: {
                                    street: '',
                                    number: '',
                                    zipcode: '',
                                    place: '',
                                },
                                driverLicenseTypeID: [],
                                vehicleID: null,
                                fuelCardID: null,
                            }}
                            onSubmit={(values) => onSubmit(values)}
                            validationSchema={CreateValidationScheme}
                        >
                            {({errors, values}) => (
                                <Form>
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
                                                        tooltip={"example: 02021518897"}
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
                                                        label="Number"
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
                                                        options={selectLists.driverLicenseTypeSelectList}
                                                        label="Rijbewijstype"
                                                        required={true}
                                                    /></div>
                                            </div>

                                            <div className="row">
                                                <div className="col-md-6">
                                                    <SelectListField
                                                        name="vehicleID"
                                                        label="Voertuig"
                                                        required={false}
                                                        selectListItems={selectLists.vehicleSelectList}
                                                    />
                                                </div>
                                                <div className="col-md-6">
                                                    <SelectListField
                                                        name="fuelCardID"
                                                        label="Tankkaart"
                                                        required={false}
                                                        selectListItems={selectLists.fuelCardSelectList}
                                                    />
                                                </div>
                                            </div>

                                            <div className="row">
                                                <div className="col-md-12">
                                                    <hr/>
                                                    <button className="btn btn-success float-end"
                                                            onClick={() => console.log(values)}
                                                            type="submit"
                                                    >
                                                        <FontAwesomeIcon icon={faSave}/> Opslaan
                                                    </button>
                                                </div>
                                            </div>

                                        </div>
                                        <div className="col-md-3"></div>
                                    </div>
                                </Form>
                            )}
                        </Formik>
                    }
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
}

export default DriverCreate;
