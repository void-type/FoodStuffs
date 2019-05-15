export default {
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
