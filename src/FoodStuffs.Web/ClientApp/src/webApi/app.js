import axios from 'axios';

function setHeaders(response) {
  const csrfTokenHeaderName = response.data.antiforgeryTokenHeaderName;
  const csrfToken = response.data.antiforgeryToken;

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
}

export default {
  getInfo(success, failure) {
    axios.get('/api/app/info')
      .then((response) => {
        setHeaders(response);
        success(response.data);
      })
      .catch(error => failure(error.response));
  },
};
