import axios from 'axios';

export default {
  getInfo(success, failure) {
    axios.get('api/app/info')
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  },
  setAntiforgeryToken(csrfTokenHeaderName, csrfToken) {
    axios.defaults.headers.get = {
      Pragma: 'no-cache',
    };
    const headers = {
      [csrfTokenHeaderName]: csrfToken,
      'X-Requested-With': 'XMLHttpRequest',
    };
    axios.defaults.headers.post = headers;
    axios.defaults.headers.put = headers;
    axios.defaults.headers.delete = headers;
  },
};
