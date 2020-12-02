import React from "react";
import { Form, InputGroup } from "react-bootstrap";
import "../styles/currency-input.css";

export interface ICurrencyInputProps {
  onChange: (value: React.ChangeEvent<HTMLInputElement>) => void;
  min?: number;
  max?: number;
  value?: number;
  label?: string;
  placeholder?: string;
  required?: boolean;
  fullWidth?: boolean;
}

const CurrencyInput: React.FC<ICurrencyInputProps> = (props: ICurrencyInputProps): JSX.Element => {
  const handleChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    const currencyRegex = /^[0-9]*(?:\.[0-9]{0,2})?$/; // Checks if value is in money format

    if (event.target.value === "") {
      event.target.value = "0";
    } else if (event.target.value[0] === ".") {
      event.target.value = "0" + event.target.value;
    } else if (event.target.value.length > 1 // Checks for 01, 02, ... cases and converts them to 1, 2, ...
      && event.target.value[0] === "0" 
      && event.target.value[1] !== "."
    ) {
      event.target.value = event.target.value.slice(1);
    }

    if (currencyRegex.test(event.target.value) ) {
      props.onChange(event);
    } 
  };

  return (
    <div className={props.fullWidth ? "w-100" : "w-50"}>
      {props.label && <Form.Label>{props.label}</Form.Label>}
      <InputGroup>
        <Form.Control
          type="number"
          min={props.min}
          max={props.max}
          step="any"
          required={props.required}
          placeholder={props.placeholder}
          value={props.value}
          onChange={handleChange}
        />
        <InputGroup.Append>
          <InputGroup.Text>€</InputGroup.Text>
        </InputGroup.Append>
      </InputGroup>
    </div>
  );
};

export default CurrencyInput;
