import User from "./User";

export default interface UserWithToken extends User {
  token: string;
}