import Select from "react-select";
import { useField, ErrorMessage } from "formik";
import React from "react";

export default function SelectField({
  label,
  required,
  options,
  ...otherprops
}) {
  const [field, state, { setValue, setTouched }] = useField(
    otherprops.field.name
  );

  // value is an array now
  const onChange = (value) => {
    setValue(value);
  };

  const convertedOptions = options.map((el) => {
    return {
      label: el.text,
      value: el.value,
    };
  });

  // use value to make this a  controlled component
  // now when the form receives a value for 'campfeatures' it will populate as expected
  return (
    <div className="m-2 form-group">
      <label htmlFor={field.name}>
        {label}
        {required ? "*" : ""}
      </label>
      <Select
        {...otherprops}
        options={convertedOptions}
        value={field.value}
        isMulti
        onChange={onChange}
        onBlur={setTouched}
        placeholder="--selecteer---"
      />
      <ErrorMessage
        className="text-danger"
        name={field.name}
        component="div"
      />
    </div>
  );
}
