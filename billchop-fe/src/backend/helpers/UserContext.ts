import { BehaviorSubject, Observable } from "rxjs";
import UserClient from "../clients/UserClient";
import { CreateNewUser } from "../models/CreateNewUser";
import { LoginDetails } from "../models/LoginDetails";
import User from "../models/User";
import UserWithToken from "../models/UserWithToken";
import axios from "axios";
import LoadingContext from "./LoadingContext";

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
      config.headers["Content-Type"] = "application/json";
      if (!this.user) {
        return config;
      } 

      config.headers.credentials = "include";
      config.headers.Authorization = `Bearer ${this.user.Token}`;

      return config;
    });

    axios.interceptors.response.use(
      (response) => response, 
      (error) => {
        if (error.response && error.response.status === 401)
          this.logout();
        
        LoadingContext.isLoading = false;
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

  public isLoggedIn = async (): Promise<boolean> => {
    if (!this.user)
      return false;

    try {
      await this.userClient.currentUser();
      return true;
    } catch {
      this.logout(); // TODO.AZ: Do this smarter next iteration (we shouldn't log out on random error or server down)
      return false;
    }
  };

  public async login(loginDetails: LoginDetails): Promise<User> {
    return this.userClient.loginUser(loginDetails).then((user) => {
      localStorage.setItem("currentUser", JSON.stringify(user));
      this.currentUserSubject.next(user);

      return user;
    });
  }

  public logout(): void {
    localStorage.removeItem("currentUser");

    if (this.user) 
      this.currentUserSubject.next(undefined);
  }

  public async register(createNewUser: CreateNewUser): Promise<void> {
    this.userClient.postUser(createNewUser)
      .then((newUser) => this.login({email: newUser.Email, password: createNewUser.password}));
  }
}

const UserContext = new UserContextManager();
UserContext.isLoggedIn(); //Do initial server ping to check stored credential validity

export default UserContext;