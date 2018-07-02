import axios from 'axios';
import recipes from './recipeApi';
import app from './appApi';

export default {
  app,
  recipes,

  setRequestVerificationToken(csrfToken) {
    const headers = {
      'X-CSRF-TOKEN': csrfToken,
      'X-Requested-With': 'XMLHttpRequest',
    };
    axios.defaults.headers.post = headers;
    axios.defaults.headers.put = headers;
    axios.defaults.headers.delete = headers;
  },

  setDocumentTitle(applicationName) {
    document.title = applicationName;
  },
};
