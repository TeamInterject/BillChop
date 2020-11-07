import User from "./User";
import Loan from "./Loan";

export default interface Bill {
  Id: string;
  Name: string;
  Total: number;
  CreationTime: string;
  Loaner: User;
  Loans: Loan[];
  GroupContextId: string;
}
