import { BehaviorSubject, Observable } from "rxjs";
import UserClient from "../clients/UserClient";
import User from "../models/User";

class UserContextManager {
  private userClient = new UserClient();

  private userJson = localStorage.getItem("currentUser");

  private currentUser: User | undefined = this.userJson
    ? JSON.parse(this.userJson)
    : undefined;

  private currentUserSubject = new BehaviorSubject(this.currentUser);

  public get user(): User | undefined {
    return this.currentUserSubject.value;
  }

  public get authenticatedUser(): User {
    if (!this.user)
      throw new Error("Got unauthenticated user, when expecting authenticated");

    return this.user;
  }

  public get userObservable(): Observable<User | undefined> {
    return this.currentUserSubject.asObservable();
  }

  public async isLoggedIn(): Promise<boolean> {
    if (!this.user) return false;

    try {
      await this.login(this.user.Email);
      return true;
    } catch (e) {
      this.logout(); // TODO.AZ: Do this smarter later (we shouldn't log out on random error or server down)
      return false;
    }
  }

  public async login(email: string): Promise<User> {
    return this.userClient.loginUser({ email }).then((user) => {
      localStorage.setItem("currentUser", JSON.stringify(user));
      this.currentUserSubject.next(user);

      return user;
    });
  }

  public logout(): void {
    localStorage.removeItem("currentUser");

    if (this.user) this.currentUserSubject.next(undefined);
  }

  public async register(name: string, email: string): Promise<boolean> {
    try {
      const newUser = await this.userClient.postUser({
        name,
        email,
      });
      await this.login(newUser.Email);
      return true;
    } catch (e) {
      return false;
    }
  }
}

const UserContext = new UserContextManager();
export default UserContext;
