import DatePicker from "react-datepicker";
import React from "react";
import {ErrorMessage} from "formik";

const DateField = (props) => {
    return (
        <div className="form-group m-2">
            <label htmlFor={props.name}>{props.label}{props.required ? '*' : ''}</label>
            <DatePicker
             name={props.name}
             selected={props.selected}
             onChange={props.onChange}
             dateFormat={"dd-MM-yyyy"}
             showMonthDropdown
             showYearDropdown
             dropdownMode="select"
             placeholderText="dd-mm-yyyy"
             className="form-control"
            />
            <ErrorMessage className="text-danger" name={props.name} component="div"/>
        </div>
    );
}

export default DateField;