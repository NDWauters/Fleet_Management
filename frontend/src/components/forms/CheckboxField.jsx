import {ErrorMessage, useField, useFormikContext} from "formik";
import React from "react";
import {ToggleSlider} from "react-toggle-slider";

const CheckBoxField = (props) => {

    const { name, label, required, value } = props;
    const { setFieldValue } = useFormikContext();
    const [field] = useField(props);

    return (
        <div className="form-group m-2">
            <label htmlFor={name}>{label}{required ? '*' : ''}</label>
           <ToggleSlider
               {...field}
               name={name}
               active={value}
               onToggle={(val) => setFieldValue(field.name, val)}
           />
            <ErrorMessage className="text-danger" name={name} component="div" />
        </div>
    );
}

export default CheckBoxField;