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
export const CURRENT_USER_ID = "b949c165-447c-4506-0ce7-08d879d159ca";
