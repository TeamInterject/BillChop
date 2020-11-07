import User from "./User";

export default interface Loan {
  Id: string;
  Amount: number;
  Loanee: User;
  Loaner: User;
  BillId: string;
}
