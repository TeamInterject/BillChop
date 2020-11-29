import url from "url-join";
import ServerConfig from "../server-config.json";

export default abstract class BaseClient {
  protected get baseUrl(): string {
    return url(ServerConfig.host, this.relativeUrl);
  }

  protected createQuery = <TType>(
    queryParamName: string,
    queryValue?: TType,
  ): string => {
    return queryValue !== null && queryValue !== undefined
      ? `?${queryParamName}=${queryValue}`
      : "";
  };

  public abstract get relativeUrl(): string;
}
