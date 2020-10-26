import User from "./User";

export default interface Loan {
  Id: string;
  Amount: number;
  LoaneeId: string;
  Loanee: User;
  BillId: string;
}
