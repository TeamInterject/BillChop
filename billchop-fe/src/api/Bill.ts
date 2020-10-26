import Group from "./Group";
import User from "./User";
import Loan from "./Loan";

export default interface Bill {
  Id: string;
  Name: string;
  Total: number;
  LoanerId: string;
  Loaner: User;
  Loans: Loan[];
  GroupContextId: string;
  GroupContext: Group;
}
