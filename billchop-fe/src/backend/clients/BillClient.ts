import Axios, { AxiosResponse } from "axios";
import url from "url-join";
import BaseClient from "./AbstractClient";
import Bill from "../models/Bill";
import { CreateNewBill } from "../models/CreateNewBill";

export default class BillClient extends BaseClient {
  public get relativeUrl(): string {
    return "api/bills";
  }

  public getBills = async (groupId?: string): Promise<Bill[]> => {
    const groupQuery = this.createQuery("groupId", groupId);

    const requestUrl = url(this.baseUrl, groupQuery);
    return Axios.get(requestUrl).then(
      (response: AxiosResponse<Bill[]>) => response.data
    );
  };

  public postBill = async (createNewBill: CreateNewBill): Promise<Bill> => {
    const requestUrl = url(this.baseUrl);
    return Axios.post(requestUrl, createNewBill).then(
      (response: AxiosResponse<Bill>) => response.data
    );
  };
}
