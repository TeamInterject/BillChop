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
    } else if (event.target.value[0] === ".") {
      event.target.value = "0" + event.target.value;
    } else if (event.target.value.length > 1 && event.target.value[0] === "0" && event.target.value[1] !== ".") { // Checks for 01, 02, ... cases
      event.target.value = event.target.value.slice(1);
    }

    const parsedNewValue = parseFloat(event.target.value);

    if (currencyRegex.test(event.target.value) && parsedNewValue >= 0 && parsedNewValue <= (props.max ?? Infinity)) {
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
