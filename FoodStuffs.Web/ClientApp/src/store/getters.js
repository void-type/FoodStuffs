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