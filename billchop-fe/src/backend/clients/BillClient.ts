import Axios, { AxiosResponse } from "axios";
import url from "url-join";
import BaseClient from "./BaseClient";
import Bill from "../models/Bill";
import { CreateNewBill } from "../models/CreateNewBill";
import LoadingContext from "../helpers/LoadingContext";

const GROUP_PARAM = "groupId";
const START_TIME_PARAM = "startTime";
const END_TIME_PARAM = "endTime";

export default class BillClient extends BaseClient {
  public get relativeUrl(): string {
    return "api/bills";
  }

  public getBills = async (props: {
    groupId?: string;
    startTime?: Date;
    endTime?: Date;
  }): Promise<Bill[]> => {
    const { groupId, startTime, endTime } = props;

    const groupQuery = this.createQuery(GROUP_PARAM, groupId);
    const startTimeQuery = this.createQuery(START_TIME_PARAM, startTime);
    const endTimeQuery = this.createQuery(END_TIME_PARAM, endTime);

    const requestUrl = url(
      this.baseUrl,
      groupQuery,
      startTimeQuery,
      endTimeQuery,
    );
    
    LoadingContext.isLoading = true;
    return Axios.get(requestUrl).then((response: AxiosResponse<Bill[]>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
  };

  public postBill = async (createNewBill: CreateNewBill): Promise<Bill> => {
    const requestUrl = url(this.baseUrl);

    LoadingContext.isLoading = true;
    return Axios.post(requestUrl, createNewBill).then((response: AxiosResponse<Bill>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
  };
}
