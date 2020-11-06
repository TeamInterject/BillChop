import Axios, { AxiosResponse } from "axios";
import url from "url-join";
import BaseClient from "./AbstractClient";
import Loan from "../models/Loan";

const LOANEE_PARAM = "loaneeId";
const LOANER_PARAM = "loanerId";
const GROUP_PARAM = "groupId";
const START_TIME_PARAM = "startTime";
const END_TIME_PARAM = "endTime";

export default class LoanClient extends BaseClient {
  public get relativeUrl(): string {
    return "api/loans";
  }

  public getProvidedLoans = async (props: {
    loanerId: string;
    groupId?: string;
    startTime?: Date;
    endTime?: Date;
  }): Promise<Loan[]> => {
    const { loanerId, groupId, startTime, endTime } = props;

    const groupQuery = this.createQuery(GROUP_PARAM, groupId);
    const startTimeQuery = this.createQuery(START_TIME_PARAM, startTime);
    const endTimeQuery = this.createQuery(END_TIME_PARAM, endTime);

    const requestUrl = url(
      this.baseUrl,
      "provided-loans",
      loanerId,
      groupQuery,
      startTimeQuery,
      endTimeQuery
    );

    return Axios.get(requestUrl).then(
      (response: AxiosResponse<Loan[]>) => response.data
    );
  };

  public getReceivedLoans = async (props: {
    loaneeId: string;
    groupId?: string;
    startTime?: Date;
    endTime?: Date;
  }): Promise<Loan[]> => {
    const { loaneeId, groupId, startTime, endTime } = props;

    const groupQuery = this.createQuery(GROUP_PARAM, groupId);
    const startTimeQuery = this.createQuery(START_TIME_PARAM, startTime);
    const endTimeQuery = this.createQuery(END_TIME_PARAM, endTime);

    const requestUrl = url(
      this.baseUrl,
      "received-loans",
      loaneeId,
      groupQuery,
      startTimeQuery,
      endTimeQuery
    );

    return Axios.get(requestUrl).then(
      (response: AxiosResponse<Loan[]>) => response.data
    );
  };

  public getSelfLoans = async (props: {
    userId: string;
    groupId?: string;
    startTime?: Date;
    endTime?: Date;
  }): Promise<Loan[]> => {
    const { userId, groupId, startTime, endTime } = props;

    const groupQuery = this.createQuery(GROUP_PARAM, groupId);
    const startTimeQuery = this.createQuery(START_TIME_PARAM, startTime);
    const endTimeQuery = this.createQuery(END_TIME_PARAM, endTime);

    const requestUrl = url(
      this.baseUrl,
      "self-loans",
      userId,
      groupQuery,
      startTimeQuery,
      endTimeQuery
    );

    return Axios.get(requestUrl).then(
      (response: AxiosResponse<Loan[]>) => response.data
    );
  };

  public getLoans = async (queryProps: {
    loaneeId?: string;
    loanerId?: string;
    groupId?: string;
    startTime?: Date;
    endTime?: Date;
  }): Promise<Loan[]> => {
    const { loaneeId, loanerId, groupId, startTime, endTime } = queryProps;

    const loaneeQuery = this.createQuery(LOANEE_PARAM, loaneeId);
    const loanerQuery = this.createQuery(LOANER_PARAM, loanerId);
    const groupQuery = this.createQuery(GROUP_PARAM, groupId);
    const startTimeQuery = this.createQuery(START_TIME_PARAM, startTime);
    const endTimeQuery = this.createQuery(END_TIME_PARAM, endTime);

    const requestUrl = url(
      this.baseUrl,
      loaneeQuery,
      loanerQuery,
      groupQuery,
      startTimeQuery,
      endTimeQuery
    );

    return Axios.get(requestUrl).then(
      (response: AxiosResponse<Loan[]>) => response.data
    );
  };
}
