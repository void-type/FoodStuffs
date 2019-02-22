import webApi from '../../../webApi';

// TODO: Pull API out of store.
export default {
  clearMessages(context) {
    context.commit('SET_MESSAGE_IS_ERROR', false);
    context.commit('SET_FIELDS_IN_ERROR', []);
    context.commit('SET_MESSAGES', []);
  },
  fetchApplicationInfo(context) {
    webApi.app.getInfo(
      (data) => {
        context.commit('SET_APPLICATION_NAME', data.applicationName);
        context.commit('SET_USER', data.user);
        webApi.app.setAntiforgeryToken(data.antiforgeryTokenHeaderName, data.antiforgeryToken);
      },
      response => context.dispatch('setApiFailureMessage', response),
    );
  },
  setApiFailureMessage(context, response) {
    if (response === undefined || response === null) {
      context.dispatch('setErrorMessage', 'Cannot connect to server.');
    } else if (response.status === 401 || response.status === 403) {
      context.dispatch('setErrorMessage', 'You are not authorized for this server endpoint.');
    } else if (response.status === 404) {
      context.dispatch('setErrorMessage', 'Server responded with endpoint not found.');
    } else if (response.status >= 500) {
      context.dispatch('setErrorMessage', response.data.message);
    } else if (response.data.items !== undefined) {
      context.dispatch('setValidationErrorMessages', {
        errorMessages: response.data.items.map(item => item.message),
        fieldNames: response.data.items.map(item => item.uiHandle),
      });
    } else {
      context.dispatch('setErrorMessage', 'Something went wrong. Try refreshing your browser or contact the administrator.');
    }
  },
  setApiDownloadFailureMessage(context, response) {
    let decodedResponse = response;
    if (response.request.responseType === 'arraybuffer') {
      const decodedString = String.fromCharCode.apply(null, new Uint8Array(response.data));
      if (decodedString.length > 0) {
        decodedResponse = Object.assign(response, { data: JSON.parse(decodedString) });
      }
    }
    context.dispatch('setApiFailureMessage', decodedResponse);
  },
  setErrorMessage(context, message) {
    context.commit('SET_MESSAGE_IS_ERROR', true);
    context.commit('SET_FIELDS_IN_ERROR', []);
    context.commit('SET_MESSAGES', [message]);
  },
  setSuccessMessage(context, message) {
    context.commit('SET_MESSAGE_IS_ERROR', false);
    context.commit('SET_FIELDS_IN_ERROR', []);
    context.commit('SET_MESSAGES', [message]);
  },
  setValidationErrorMessages(context, { errorMessages, fieldNames }) {
    context.commit('SET_MESSAGE_IS_ERROR', true);
    context.commit('SET_FIELDS_IN_ERROR', fieldNames);
    context.commit('SET_MESSAGES', errorMessages);
  },
};
