export default {
  applicationName(state) {
    return state.applicationName;
  },
  isFieldInError: state => fieldName => state.fieldsInError
    .map(errorField => errorField.toLowerCase())
    .indexOf(fieldName.toLowerCase()) > -1,
  messageIsError(state) {
    return state.messageIsError;
  },
  messages(state) {
    return state.messages;
  },
  user(state) {
    return state.user;
  },
  userIsAuthorizedAs: state => policy => state.user.authorizedAs.includes(policy),
};
