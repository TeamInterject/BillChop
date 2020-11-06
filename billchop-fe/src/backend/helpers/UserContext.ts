import UserClient from "../clients/UserClient";

// TODO: Refactor this out
export default class UserContext {
  private static currentUserId?: string;

  private static userClient = new UserClient();

  public static async getOrCreateTestUser(): Promise<string> {
    if (UserContext.currentUserId) return UserContext.currentUserId;

    const users = await this.userClient.getUsers();
    if (users.length !== 0) {
      UserContext.currentUserId = users[0].Id;
      return UserContext.currentUserId;
    }

    const user = await this.userClient.postUser({
      name: "Name Namingson",
      email: "name.namingson@gmail.com",
    });

    UserContext.currentUserId = user.Id;
    return UserContext.currentUserId;
  }
}
