import Axios, { AxiosResponse } from "axios";
import url from "url-join";
import BaseClient from "./AbstractClient";
import Group from "../models/Group";
import { CreateNewGroup } from "../models/CreateNewGroup";

const USER_PARAM = "userId";

export default class GroupClient extends BaseClient {
  public get relativeUrl(): string {
    return "api/groups";
  }

  public getGroups = async (userId?: string): Promise<Group[]> => {
    const userQuery = this.createQuery(USER_PARAM, userId);
    const requestUrl = url(this.baseUrl, userQuery);

    return Axios.get(requestUrl).then(
      (response: AxiosResponse<Group[]>) => response.data,
    );
  };

  public getGroup = async (groupId: string): Promise<Group> => {
    const requestUrl = url(this.baseUrl, groupId);

    return Axios.get(requestUrl).then(
      (response: AxiosResponse<Group>) => response.data,
    );
  };

  public postGroup = async (createNewGroup: CreateNewGroup): Promise<Group> => {
    const requestUrl = url(this.baseUrl);

    return Axios.post(requestUrl, createNewGroup).then(
      (response: AxiosResponse<Group>) => response.data,
    );
  };

  public addUserToGroup = async (
    groupId: string,
    userId: string,
  ): Promise<Group> => {
    const requestUrl = url(this.baseUrl, groupId, "add-user", userId);

    return Axios.post(requestUrl).then(
      (response: AxiosResponse<Group>) => response.data,
    );
  };
}
