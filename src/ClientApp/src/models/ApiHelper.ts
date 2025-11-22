import type { ApiConfig } from '../api/http-client';
import { Api } from '../api/Api';

const defaultHeaders: Record<string, string> = {};

export default class ApiHelper {
  static setHeader(headerName: string, headerValue: string) {
    defaultHeaders[headerName] = headerValue;
  }

  static client(apiConfig?: ApiConfig<unknown>) {
    if (apiConfig) {
      return new Api(apiConfig);
    }

    return new Api({
      baseApiParams: {
        headers: defaultHeaders,
      },
    });
  }

  static imageUrl(fileName: number | string) {
    return `/api/images/${fileName}`;
  }
}
