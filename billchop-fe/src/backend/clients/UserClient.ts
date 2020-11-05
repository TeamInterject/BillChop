import Axios, { AxiosResponse } from "axios";
import url from "url-join";
import User from "../models/User";
import { CreateNewUser } from "../models/CreateNewUser";
import BaseClient from "./AbstractClient";

export default class UserClient extends BaseClient {
  public get relativeUrl(): string {
    return "api/users";
  }

  public getUsers = async (): Promise<User[]> => {
    const requestUrl = this.baseUrl;
    return Axios.get(requestUrl).then(
      (response: AxiosResponse<User[]>) => response.data
    );
  };

  public getUser = async (id: string): Promise<User> => {
    const requestUrl = url(this.baseUrl, id);
    return Axios.get(requestUrl).then(
      (response: AxiosResponse<User>) => response.data
    );
  };

  public postUser = async (createNewUser: CreateNewUser): Promise<User> => {
    const requestUrl = url(this.baseUrl);
    return Axios.post(requestUrl, createNewUser).then(
      (response: AxiosResponse<User>) => response.data
    );
  };
}
