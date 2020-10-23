import Group from "./Group";

export default interface User {
  id: string;
  name: string;
  groups: Group[];
}
