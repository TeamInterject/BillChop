import url from "url-join";
import ServerConfig from "../server-config.json";

export default abstract class BaseClient {
  protected get baseUrl(): string {
    return url(ServerConfig.host, this.relativeUrl);
  }

  protected createQuery = (
    queryParamName: string,
    queryValue?: string
  ): string => {
    return queryValue !== null && queryValue !== undefined
      ? `?${queryParamName}=${queryValue}`
      : "";
  };

  public abstract get relativeUrl(): string;
}
