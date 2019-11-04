import downloadHelpers from '../../../util/downloadHelpers';

export default {
  setApiFailureMessages(context, response) {
    if (response === undefined || response === null) {
      context.dispatch('setErrorMessage', 'Cannot connect to server.');
      return;
    }

    const data = (response.request.responseType !== 'arraybuffer')
      ? response.data
      : downloadHelpers.decodeDownloadFailureData(response);

    if (response.status === 401 || response.status === 403) {
      context.dispatch('setErrorMessage', 'You are not authorized for this server endpoint.');
    } else if (response.status === 404) {
      context.dispatch('setErrorMessage', 'Server responded with endpoint not found.');
    } else if (response.status >= 500) {
      context.dispatch('setErrorMessage', data.message);
    } else if (data.items !== undefined) {
      context.dispatch('setValidationErrorMessages', {
        errorMessages: data.items.map(item => item.message),
        fieldNames: data.items.map(item => item.uiHandle),
      });
    } else {
      context.dispatch('setErrorMessage', 'Something went wrong. Try refreshing your browser or contact the administrator.');
    }
  },
  clearMessages(context) {
    context.commit('SET_MESSAGE_IS_ERROR', false);
    context.commit('SET_FIELDS_IN_ERROR', []);
    context.commit('SET_MESSAGES', []);
  },
  setApplicationInfo(context, data) {
    context.commit('SET_APPLICATION_NAME', data.applicationName);
    context.commit('SET_USER', data.user);
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
    function isNullOrEmpty(item) {
      return item === null || item === undefined || item === '';
    }

    context.commit('SET_MESSAGE_IS_ERROR', true);
    context.commit('SET_FIELDS_IN_ERROR', fieldNames.filter(s => !isNullOrEmpty(s)));
    context.commit('SET_MESSAGES', errorMessages.filter(s => !isNullOrEmpty(s)));
  },
};
