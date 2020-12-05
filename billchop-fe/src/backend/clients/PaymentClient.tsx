import Axios, { AxiosResponse } from "axios";
import url from "url-join";
import LoadingContext from "../helpers/LoadingContext";
import CreateNewPayment from "../models/CreateNewPayment";
import Payment from "../models/Payment";
import BaseClient from "./BaseClient";

const GROUP_PARAM = "groupId";

export default class PaymentClient extends BaseClient {
  public get relativeUrl(): string {
    return "api/payments";
  }

  public getExpectedPayments = async (props: {
    userId: string;
    groupId?: string;
  }): Promise<Payment[]> => {
    const { userId, groupId } = props;

    const groupQuery = this.createQuery(GROUP_PARAM, groupId);

    const requestUrl = url(
      this.baseUrl,
      "user",
      userId,
      groupQuery,
    );

    LoadingContext.isLoading = true;
    return Axios.get(requestUrl).then((response: AxiosResponse<Payment[]>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
  };

  public createPayment = async (createNewPayment: CreateNewPayment): Promise<Payment> => {
    const requestUrl = url(
      this.baseUrl,
    );

    LoadingContext.isLoading = true;
    return Axios.post(requestUrl, createNewPayment).then((response: AxiosResponse<Payment>) => {
      LoadingContext.isLoading = false;
      return response.data;
    });
  };
}
