import React from "react";
import styled from "styled-components";

const Select = styled.select.attrs(props => ({
    type: "text",
    size: props.small ? 5 : undefined
}))`
  height: 32px;
  width: 6vw;
  border-radius: 5px 0 0 5px;
  border: 1px solid #e5e5e5;
  padding: 0 32px 0 16px;
`;

const ClearButton = styled.button`
  border-radius: 0 5px 5px 0;
  margin-right: 15px;
  height: 32px;
  width: 2vw;
  text-align: center;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 1px solid #e5e5e8;
`;

const FilterComponent = ({ value, onChange, onClear, label, options }) => (
    <div className="d-flex flex-column">
        <label>{label}</label>
        <div className="d-flex flex-row">
            <Select
                id="search"
                onChange={onChange}
                options
            >
                {
                    options.map((el, i) => <option key={i} selected={value === el.value} value={el.value}>{el.text}</option>)
                }
            </Select>
            <ClearButton onClick={onClear}>X</ClearButton>
        </div>
    </div>
);

export default FilterComponent;
