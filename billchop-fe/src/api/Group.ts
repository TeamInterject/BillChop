import User from "./User";

import Bill from "./Bill";

export default interface Group {
  Id: string;
  Name: string;
  Users: User[];
  Bills: Bill[];
}
