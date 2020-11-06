import Axios, { AxiosResponse } from "axios";
import url from "url-join";
import BaseClient from "./AbstractClient";
import Loan from "../models/Loan";

const loaneeParam = "loaneeId";
const loanerParam = "loanerId";
const groupParam = "groupId";
const startTimeParam = "startTime";
const endTimeParam = "endTime";

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

    const groupQuery = this.createQuery(groupParam, groupId);
    const startTimeQuery = this.createQuery(startTimeParam, startTime);
    const endTimeQuery = this.createQuery(endTimeParam, endTime);

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

    const groupQuery = this.createQuery(groupParam, groupId);
    const startTimeQuery = this.createQuery(startTimeParam, startTime);
    const endTimeQuery = this.createQuery(endTimeParam, endTime);

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

    const groupQuery = this.createQuery(groupParam, groupId);
    const startTimeQuery = this.createQuery(startTimeParam, startTime);
    const endTimeQuery = this.createQuery(endTimeParam, endTime);

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

    const loaneeQuery = this.createQuery(loaneeParam, loaneeId);
    const loanerQuery = this.createQuery(loanerParam, loanerId);
    const groupQuery = this.createQuery(groupParam, groupId);
    const startTimeQuery = this.createQuery(startTimeParam, startTime);
    const endTimeQuery = this.createQuery(endTimeParam, endTime);

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
