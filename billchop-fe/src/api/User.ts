import Group from "./Group";
import Loan from "./Loan";
import Bill from "./Bill";

export default interface User {
  Id: string;
  Name: string;
  Groups: Group[];
  Loans: Loan[];
  Bills: Bill[];
}

//TODO refactor this out
export const CURRENT_USER_ID = "4829242d-7e32-4ba1-6a51-08d87986475f";
