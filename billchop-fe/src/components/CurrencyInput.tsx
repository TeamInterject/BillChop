import React, { useEffect, useRef } from "react";
import { Form } from "react-bootstrap";
import "../styles/currency-input.css";

export interface ICurrencyInputProps {
  onChange: (value: React.ChangeEvent<HTMLInputElement>) => void,
  max?: number,
  value?: number,
}

const CurrencyInput: React.FC<ICurrencyInputProps> = (props: ICurrencyInputProps): JSX.Element => {
  const previousValue = usePrevious(props.value ?? 0.00);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    const newValue = event.target.value;
    const regex = /[0-9]*(?:\.[0-9]{2})?/;
    const parsedNewValue = parseFloat(event.target.value);

    if (regex.test(newValue) && props.max && parsedNewValue <= props.max && parsedNewValue >= 0) {
      props.onChange(event);
    } else if (newValue === "") {
      event.target.value = "0.00";
    } else {
      event.target.value = previousValue.toString();
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

function usePrevious(value: number) {
  const ref = useRef(0.00);
  
  useEffect(() => {
    ref.current = value;
  }, [value]);
  
  return ref.current;
}

export default CurrencyInput;
