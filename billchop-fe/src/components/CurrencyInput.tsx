import React from "react";
import { Form } from "react-bootstrap";
import "../styles/currency-input.css";

export interface ICurrencyInputProps {
  onChange: (value: React.ChangeEvent<HTMLInputElement>) => void,
  max?: number,
  value?: number,
}

const CurrencyInput: React.FC<ICurrencyInputProps> = (props: ICurrencyInputProps): JSX.Element => {
  const handleChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    const currencyRegex = /^[0-9]*(?:\.[0-9]{0,2})?$/; // Checks if value is in money format

    if (event.target.value === "") {
      event.target.value = "0";
    }
    const newValue = event.target.value;
    const parsedNewValue = parseFloat(newValue);

    if (currencyRegex.test(newValue) && parsedNewValue >= 0 && parsedNewValue <= (props.max ?? Infinity)) {
      if (newValue.length > 1 && newValue[0] === "0" && newValue[1] !== ".") { // Checks for 01, 02, ... cases
        event.target.value = newValue.slice(1);
      }
      props.onChange(event);
    } 
  };

  return (
    <Form.Control
      type="number"
      value={props.value}
      onChange={handleChange}
    />
  );
};

export default CurrencyInput;
