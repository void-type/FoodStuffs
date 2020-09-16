import axios from 'axios';

function setHeaders(response) {
  const csrfTokenHeaderName = response.data.antiforgeryTokenHeaderName;
  const csrfToken = response.data.antiforgeryToken;

  const headers = {
    [csrfTokenHeaderName]: csrfToken,
    'X-Requested-With': 'XMLHttpRequest',
  };

  axios.defaults.headers.post = headers;
  axios.defaults.headers.put = headers;
  axios.defaults.headers.delete = headers;
}

export default {
  getInfo(success, failure) {
    // Set the get header to be no-cache for all requests.
    axios.defaults.headers.get = {
      Pragma: 'no-cache',
    };

    axios.get('/api/app/info')
      .then((response) => {
        setHeaders(response);
        success(response.data);
      })
      .catch((error) => failure(error.response));
  },
  getVersion(success, failure) {
    axios.get('/api/app/version')
      .then((response) => {
        setHeaders(response);
        success(response.data);
      })
      .catch((error) => failure(error.response));
  },
};
