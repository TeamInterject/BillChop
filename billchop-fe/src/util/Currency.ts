const toEuros = (amount: number): string => {
  return `${amount.toFixed(2)}€`.replace("-0.00", "0.00");
};

export default toEuros;
