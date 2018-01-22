export default {
  clearErrors(state) {
    state.isError = false;
    state.fieldsInError = [];
    state.messages = [];
  },

  selectRecipe(state, recipe) {
    state.currentRecipe = recipe;
  },

  setRecipesList(state, recipes) {
    state.recipes = recipes;
  },

  setMessage(state, message) {
    state.messages = [message];
  },

  setMessages(state, messages) {
    state.messages = messages;
  },

  setFieldsInError(state, fieldNames) {
    state.fieldsInError = fieldNames;
  },

  setIsError(state, status) {
    state.isError = status || true;
  }
}