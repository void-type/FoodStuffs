import webApi from '../../../webApi';

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
      },
      response => webApi.showFailureMessages(response),
    );
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
