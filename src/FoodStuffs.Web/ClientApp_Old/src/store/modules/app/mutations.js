/* eslint-disable no-param-reassign */
export default {
  SET_APPLICATION_NAME(state, applicationName) {
    state.applicationName = applicationName;
  },
  SET_FIELDS_IN_ERROR(state, fieldNames) {
    state.fieldsInError = fieldNames;
  },
  SET_MESSAGES(state, messages) {
    state.messages = messages;
  },
  SET_MESSAGE_IS_ERROR(state, status) {
    state.messageIsError = status;
  },
  SET_USER(state, user) {
    state.user = user;
  },
  SET_INITIALIZED(state, isInitialized) {
    state.initialized = isInitialized;
  },
};
/* eslint-enable no-param-reassign */
