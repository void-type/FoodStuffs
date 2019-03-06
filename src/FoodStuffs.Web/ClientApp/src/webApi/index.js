import store from '../store';
import app from './app';
import recipes from './recipes';

export default {
  app,
  recipes,
  setApiFailureMessage(response) {
    if (response === undefined || response === null) {
      store.dispatch('app/setErrorMessage', 'Cannot connect to server.');
    } else if (response.status === 401 || response.status === 403) {
      store.dispatch('app/setErrorMessage', 'You are not authorized for this server endpoint.');
    } else if (response.status === 404) {
      store.dispatch('app/setErrorMessage', 'Server responded with endpoint not found.');
    } else if (response.status >= 500) {
      store.dispatch('app/setErrorMessage', response.data.message);
    } else if (response.data.items !== undefined) {
      store.dispatch('app/setValidationErrorMessages', {
        errorMessages: response.data.items.map(item => item.message),
        fieldNames: response.data.items.map(item => item.uiHandle),
      });
    } else {
      store.dispatch('app/setErrorMessage', 'Something went wrong. Try refreshing your browser or contact the administrator.');
    }
  },
};
