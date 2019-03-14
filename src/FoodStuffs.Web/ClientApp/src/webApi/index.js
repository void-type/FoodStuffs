import store from '../store';
import app from './app';
import recipes from './recipes';

export default {
  app,
  recipes,
  showFailureMessages(response) {
    const data = (response.request.responseType !== 'arraybuffer') ?
      response.data :
      this.decodeDownloadFailureData(response);

    if (response === undefined || response === null) {
      store.dispatch('app/setErrorMessage', 'Cannot connect to server.');
    } else if (response.status === 401 || response.status === 403) {
      store.dispatch('app/setErrorMessage', 'You are not authorized for this server endpoint.');
    } else if (response.status === 404) {
      store.dispatch('app/setErrorMessage', 'Server responded with endpoint not found.');
    } else if (response.status >= 500) {
      store.dispatch('app/setErrorMessage', data.message);
    } else if (data.items !== undefined) {
      store.dispatch('app/setValidationErrorMessages', {
        errorMessages: data.items.map(item => item.message),
        fieldNames: data.items.map(item => item.uiHandle),
      });
    } else {
      store.dispatch('app/setErrorMessage', 'Something went wrong. Try refreshing your browser or contact the administrator.');
    }
  },
  saveDownloadedFile(response) {
    const filename = response.headers['content-disposition']
      .split('; ')
      .filter(part => part.startsWith('filename'))[0]
      .split('=')[1];

    const contentType = response.headers['content-type'];
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
      window.navigator.msSaveBlob(blob, filename);
    }
  },
  decodeDownloadFailureData(response) {
    const decodedString = String.fromCharCode.apply(null, new Uint8Array(response.data));

    if (decodedString.length <= 0) {
      return {};
    }

    return JSON.parse(decodedString);
  },
};
