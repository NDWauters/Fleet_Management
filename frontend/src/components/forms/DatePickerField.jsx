import {ErrorMessage, Field, useField, useFormikContext} from "formik";
import React, {useState} from "react";
import { OverlayTrigger, Tooltip } from 'react-bootstrap';
import "../../styles/form.css";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCircleInfo} from "@fortawesome/free-solid-svg-icons";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import 'bootstrap/dist/css/bootstrap.min.css';

const DatePickerField = (props) => {

    let toolTipClassName;
    let fontAwesomeClassName;
    if (props.tooltip == "null"){
        toolTipClassName = "hidden";
        fontAwesomeClassName = "hidden";
    }else{
        toolTipClassName = "tooltip-control";
        fontAwesomeClassName = "fontAwesome";
    }

    const { setFieldValue } = useFormikContext();
    const [field] = useField(props);

    return (

        <div className="form-group m-2">

            <span>
                <label htmlFor={props.name}>{props.label}{props.required ? '*' : ''}</label>
                <OverlayTrigger
                    placement="top" overlay={
                    <Tooltip className={toolTipClassName}>{props.tooltip}</Tooltip>
                }
                >
                    <i className={fontAwesomeClassName}>
                        <FontAwesomeIcon icon={faCircleInfo} />
                    </i>
                </OverlayTrigger>
            </span>

                <div>

                    <DatePicker
                        {...field}
                        name={props.name}
                        selected={props.selected}
                        onChange={(date) => {
                            props.onChange(date);
                            setFieldValue(field.name, date, true);
                        }}
                        dateFormat={"dd-MM-yyyy"}
                        showMonthDropdown
                        showYearDropdown
                        dropdownMode="select"
                        placeholderText="dd-mm-yyyy"
                        className="form-control"
                        autoComplete="off"
                    />

                </div>
            <ErrorMessage className="text-danger" name={props.name} component="div"/>

        </div>

    );
}

export default DatePickerField ;