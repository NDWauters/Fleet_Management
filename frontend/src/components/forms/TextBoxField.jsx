import {ErrorMessage, Field} from "formik";
import React from "react";
import {Button, OverlayTrigger, Tooltip} from 'react-bootstrap';
import "../../styles/form.css";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import { faCircleInfo } from "@fortawesome/free-solid-svg-icons";

const TextBoxField = ({ name, label, required, type = 'text' , tooltip}) => {

    let toolTipClassName;
    let fontAwesomeClassName;
    if (!tooltip){
        toolTipClassName = "hidden";
        fontAwesomeClassName = "hidden";
    }else{
        toolTipClassName = "tooltip-control";
        fontAwesomeClassName = "fontAwesome";
    }

    return (
        <div className="form-group m-2">

            <span>
                <label htmlFor={name}>{label}{required ? '*' : ''}</label>
                <OverlayTrigger
                    placement="top" overlay={
                    <Tooltip className={toolTipClassName}>{tooltip}</Tooltip>
                    }
                >
                    <i className={fontAwesomeClassName}>
                        <FontAwesomeIcon icon={faCircleInfo} />
                    </i>
                </OverlayTrigger>
            </span>

            <Field className="form-control" type={type} name={name} placeholder={label}/>

            <ErrorMessage className="text-danger" name={name} component="div"/>

        </div>
    );
}

export default TextBoxField;