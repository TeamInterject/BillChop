import { BehaviorSubject, Observable } from "rxjs";
import UserClient from "../clients/UserClient";
import { CreateNewUser } from "../models/CreateNewUser";
import { LoginDetails } from "../models/LoginDetails";
import User from "../models/User";
import UserWithToken from "../models/UserWithToken";
import axios from "axios";

class UserContextManager {
  private userClient = new UserClient();
  private userJson = localStorage.getItem("currentUser");
  private currentUser: UserWithToken | undefined = this.userJson ? JSON.parse(this.userJson) : undefined;
  private currentUserSubject = new BehaviorSubject(this.currentUser);

  constructor() {
    this.intializeAxios();
  }

  intializeAxios(): void {
    axios.interceptors.request.use((config) => {
      if (!this.user) {
        config.headers.credentials = "";
        config.headers.Authorization = "";
        return config;
      }

      config.headers.credentials = "include";
      config.headers.Authorization = `Bearer ${this.user.token}`;
      return config;
    });

    axios.interceptors.response.use(
      (response) => response, 
      (error) => {
        if (error.response && error.response.status === 401) {
          this.logout();
          return;
        }
        
        return Promise.reject(error);
      });
  }

  public get user(): UserWithToken | undefined {
    return this.currentUserSubject.value;
  }

  public get authenticatedUser(): UserWithToken {
    if (!this.user)
      throw new Error("Got unauthenticated user, when expecting authenticated");

    return this.user;
  }

  public get userObservable(): Observable<UserWithToken | undefined> {
    return this.currentUserSubject.asObservable();
  }

  public async isLoggedIn(): Promise<boolean> {
    if (!this.user) return false;

    try {
      await this.userClient.currentUser();
      return true;
    } catch (e) {
      this.logout(); // TODO.AZ: Do this smarter later (we shouldn't log out on random error or server down)
      return false;
    }
  }

  public async login(loginDetails: LoginDetails): Promise<User> {
    return this.userClient.loginUser(loginDetails).then((user) => {
      localStorage.setItem("currentUser", JSON.stringify(user));
      this.currentUserSubject.next(user);

      return user;
    });
  }

  public logout(): void {
    localStorage.removeItem("currentUser");

    if (this.user) this.currentUserSubject.next(undefined);
  }

  public async register(createNewUser: CreateNewUser): Promise<boolean> {
    try {
      const newUser = await this.userClient.postUser(createNewUser);
      await this.login({email: newUser.Email, password: createNewUser.password});
      return true;
    } catch (e) {
      return false;
    }
  }
}

const UserContext = new UserContextManager();
export default UserContext;