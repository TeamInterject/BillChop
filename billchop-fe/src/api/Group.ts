import User from "./User";

export default interface Group {
  Id: string;
  Name: string;
  Users: User[];
}
