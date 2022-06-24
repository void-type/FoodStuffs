import type { HttpResponse } from '@/api/http-client';

// For IE compatibility
declare global {
  interface Navigator {
    // eslint-disable-next-line no-unused-vars
    msSaveBlob?: (blob: Blob, defaultName?: string) => boolean;
  }
}

export default class DownloadHelpers {
  static saveDownloadedFile(response: HttpResponse<Blob, unknown>) {
    const headerValue = response.headers.get('content-disposition') || '';

    let filename = headerValue
      .split('; ')
      .filter((part) => part.startsWith('filename='))[0]
      .split('=')[1];

    if (filename.startsWith('"')) {
      filename = filename.slice(1, -1);
    }

    const contentType = response.headers.get('content-type') || '';
    const blob = new Blob([response.data], { type: contentType });

    try {
      const linkElement = document.createElement('a');
      linkElement.setAttribute('href', window.URL.createObjectURL(blob));
      linkElement.setAttribute('download', filename);

      const clickEvent = new MouseEvent('click', {
        view: window,
        bubbles: true,
        cancelable: false,
      });

      linkElement.dispatchEvent(clickEvent);
    } catch (e) {
      // For IE compatibility
      if (window.navigator.msSaveBlob) {
        window.navigator.msSaveBlob(blob, filename);
      }
    }
  }
}
