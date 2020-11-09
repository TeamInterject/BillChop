import Axios, { AxiosResponse } from "axios";
import url from "url-join";
import User from "../models/User";
import { CreateNewUser } from "../models/CreateNewUser";
import BaseClient from "./AbstractClient";
import { LoginDetails } from "../models/LoginDetails";

const TOP_PARAM = "top";
const EXCLUSION_GROUP_PARAM = "exclusionGroupId";

export default class UserClient extends BaseClient {
  public get relativeUrl(): string {
    return "api/users";
  }

  public getUsers = async (): Promise<User[]> => {
    const requestUrl = this.baseUrl;
    return Axios.get(requestUrl).then(
      (response: AxiosResponse<User[]>) => response.data,
    );
  };

  public getUser = async (id: string): Promise<User> => {
    const requestUrl = url(this.baseUrl, id);
    return Axios.get(requestUrl).then(
      (response: AxiosResponse<User>) => response.data,
    );
  };

  public searchUserByKeyword = async (props: {
    keyword: string;
    exclusionGroupId?: string;
    top?: number;
  }): Promise<User[]> => {
    const { keyword, exclusionGroupId, top } = props;

    const topQuery = this.createQuery(TOP_PARAM, top);
    const exclusionGroupQuery = this.createQuery(EXCLUSION_GROUP_PARAM, exclusionGroupId);

    const requestUrl = url(this.baseUrl, "search", keyword, exclusionGroupQuery, topQuery);

    return Axios.get(requestUrl).then(
      (response: AxiosResponse<User[]>) => response.data,
    );
  };

  public postUser = async (createNewUser: CreateNewUser): Promise<User> => {
    const requestUrl = url(this.baseUrl);

    return Axios.post(requestUrl, createNewUser).then(
      (response: AxiosResponse<User>) => response.data,
    );
  };

  public loginUser = async (loginDetails: LoginDetails): Promise<User> => {
    const requestUrl = url(this.baseUrl, "login");

    return Axios.post(requestUrl, loginDetails).then(
      (response: AxiosResponse<User>) => response.data,
    );
  };
}
