import axios from 'axios';
import recipes from './recipeApi';
import app from './appApi';

export default {
  recipes,
  app,

  setRequestVerificationToken(csrfToken) {
    const headers = {
      'X-CSRF-TOKEN': csrfToken,
      'X-Requested-With': 'XMLHttpRequest',
    };
    axios.defaults.headers.post = headers;
    axios.defaults.headers.put = headers;
    axios.defaults.headers.delete = headers;
  },

  setTitle(applicationName) {
    document.title = applicationName;
  },
};
