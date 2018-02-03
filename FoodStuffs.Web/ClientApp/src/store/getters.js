export default {
  messages(state) {
    return state.messages;
  },
  recipes(state) {
    return state.recipes;
  },
  fieldsInError(state) {
    return state.fieldsInError;
  },
  isFieldInError: (state) => (fieldName) => {
    return state.fieldsInError.indexOf(fieldName) > -1;
  },
  currentRecipe(state) {
    return state.currentRecipe;
  },
  isError(state) {
    return state.isError;
  },
  recentRecipes(state) {
    return state.recentRecipes;
  }
}