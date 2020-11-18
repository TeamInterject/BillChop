import Axios, { AxiosResponse } from "axios";
import url from "url-join";
import User from "../models/User";
import { CreateNewUser } from "../models/CreateNewUser";
import BaseClient from "./AbstractClient";
import { LoginDetails } from "../models/LoginDetails";
import LoadingContext from "../helpers/LoadingContext";

const TOP_PARAM = "top";
const EXCLUSION_GROUP_PARAM = "exclusionGroupId";

export default class UserClient extends BaseClient {
  public get relativeUrl(): string {
    return "api/users";
  }

  public getUsers = async (): Promise<User[]> => {
    const requestUrl = this.baseUrl;

    LoadingContext.isLoading = true;
    return Axios.get(requestUrl).then((response: AxiosResponse<User[]>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
  };

  public getUser = async (id: string): Promise<User> => {
    const requestUrl = url(this.baseUrl, id);

    LoadingContext.isLoading = true;
    return Axios.get(requestUrl).then((response: AxiosResponse<User>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
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

    LoadingContext.isLoading = true;
    return Axios.get(requestUrl).then((response: AxiosResponse<User[]>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
  };

  public postUser = async (createNewUser: CreateNewUser): Promise<User> => {
    const requestUrl = url(this.baseUrl);

    LoadingContext.isLoading = true;
    return Axios.post(requestUrl, createNewUser).then((response: AxiosResponse<User>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
  };

  public loginUser = async (loginDetails: LoginDetails): Promise<User> => {
    const requestUrl = url(this.baseUrl, "login");

    LoadingContext.isLoading = true;
    return Axios.post(requestUrl, loginDetails).then((response: AxiosResponse<User>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
  };
}
