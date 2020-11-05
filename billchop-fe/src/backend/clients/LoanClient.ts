import Axios, { AxiosResponse } from "axios";
import url from "url-join";
import BaseClient from "./AbstractClient";
import Loan from "../models/Loan";

export default class LoanClient extends BaseClient {
  public get relativeUrl(): string {
    return "api/loans";
  }

  public getProvidedLoans = async (
    loanerId: string,
    groupId?: string
  ): Promise<Loan[]> => {
    const groupQuery = this.createQuery("groupId", groupId);
    const requestUrl = url(
      this.baseUrl,
      "provided-loans",
      loanerId,
      groupQuery
    );

    return Axios.get(requestUrl).then(
      (response: AxiosResponse<Loan[]>) => response.data
    );
  };

  public getReceivedLoans = async (
    loaneeId: string,
    groupId?: string
  ): Promise<Loan[]> => {
    const groupQuery = this.createQuery("groupId", groupId);
    const requestUrl = url(
      this.baseUrl,
      "received-loans",
      loaneeId,
      groupQuery
    );

    return Axios.get(requestUrl).then(
      (response: AxiosResponse<Loan[]>) => response.data
    );
  };

  public getSelfLoans = async (
    userId: string,
    groupId?: string
  ): Promise<Loan[]> => {
    const groupQuery = this.createQuery("groupId", groupId);
    const requestUrl = url(this.baseUrl, "self-loans", userId, groupQuery);

    return Axios.get(requestUrl).then(
      (response: AxiosResponse<Loan[]>) => response.data
    );
  };

  public getLoans = async (
    loaneeId?: string,
    loanerId?: string,
    groupId?: string
  ): Promise<Loan[]> => {
    const loaneeQuery = this.createQuery("loaneeId", loaneeId);
    const loanerQuery = this.createQuery("loanerId", loanerId);
    const groupQuery = this.createQuery("groupId", groupId);

    const requestUrl = url(this.baseUrl, loaneeQuery, loanerQuery, groupQuery);
    return Axios.get(requestUrl).then(
      (response: AxiosResponse<Loan[]>) => response.data
    );
  };
}
