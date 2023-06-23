import { ErrorMessage, Field } from "formik";
import React from "react";

const SelectListField = ({ selectListItems, name, label, required }) => {

  return (
    <div className="m-2 form-group">
      <label htmlFor={name}>
        {label}
        {required ? "*" : ""}
      </label>
      <Field className="form-control" as="select" name={name}>
        <option value="">--- selecteer ---</option>
        {selectListItems.map((el, index) => {
          return (
            <option key={index} value={el.value}>
              {el.text}
            </option>
          );
        })}
      </Field>
      <ErrorMessage className="text-danger" name={name} component="div" />
    </div>
  );
};

export default SelectListField;
