import User from "./User";

export default interface Payment {
  Id: string;
  Amount: number;
  CreationTime: string;
  Payer: User;
  Receiver: User;
  GroupContextId: string;
}
