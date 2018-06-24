import axios from 'axios';
import recipes from './recipeApi';
import app from './appApi';

export default {
  recipes,
  app,

  onFailure(context, response) {
    context.commit('setMessageIsError', true);
    if (response === undefined || response === null) {
      context.commit('setMessage', 'Cannot connect to server.');
    } else if (response.status >= 500) {
      context.commit('setMessage', response.data.message);
    } else {
      context.commit('setMessages', response.data.items.map(item => item.errorMessage));
      context.commit('setFieldsInError', response.data.items.map(item => item.fieldName));
    }
  },

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
