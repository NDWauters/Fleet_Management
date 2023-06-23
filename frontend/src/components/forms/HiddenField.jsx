import React from 'react';
import {Field} from "formik";

const HiddenInput = ({ name }) => {
    return (
        <input type="hidden" name={name} />
    );
}

const HiddenField = ({ name }) => {
    return (
        <Field component={HiddenInput}  name={name} />
    );
}

export default HiddenField;