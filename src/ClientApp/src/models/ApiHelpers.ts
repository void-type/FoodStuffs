import { Api } from '../api/Api';
import type { ApiConfig } from '../api/http-client';

const defaultHeaders: Record<string, string> = {};

export default class ApiHelpers {
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

  static imageUrl(id: number | string) {
    return `/api/images/${id}`;
  }
}
