import Axios, { AxiosResponse } from "axios";
import url from "url-join";
import BaseClient from "./AbstractClient";
import Group from "../models/Group";
import { CreateNewGroup } from "../models/CreateNewGroup";
import LoadingContext from "../helpers/LoadingContext";

const USER_PARAM = "userId";

export default class GroupClient extends BaseClient {
  public get relativeUrl(): string {
    return "api/groups";
  }

  public getGroups = async (userId?: string): Promise<Group[]> => {
    const userQuery = this.createQuery(USER_PARAM, userId);
    const requestUrl = url(this.baseUrl, userQuery);

    LoadingContext.isLoading = true;
    return Axios.get(requestUrl).then((response: AxiosResponse<Group[]>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
  };

  public getGroup = async (groupId: string): Promise<Group> => {
    const requestUrl = url(this.baseUrl, groupId);

    LoadingContext.isLoading = true;
    return Axios.get(requestUrl).then((response: AxiosResponse<Group>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
  };

  public postGroup = async (createNewGroup: CreateNewGroup): Promise<Group> => {
    const requestUrl = url(this.baseUrl);

    LoadingContext.isLoading = true;
    return Axios.post(requestUrl, createNewGroup).then((response: AxiosResponse<Group>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
  };

  public addUserToGroup = async (
    groupId: string,
    userId: string,
  ): Promise<Group> => {
    const requestUrl = url(this.baseUrl, groupId, "add-user", userId);

    LoadingContext.isLoading = true;
    return Axios.post(requestUrl).then((response: AxiosResponse<Group>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
  };
}
