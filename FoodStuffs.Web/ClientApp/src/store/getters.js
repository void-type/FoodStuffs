export default {
  messages(state) {
    return state.messages;
  },

  recipes(state) {
    return state.recipes;
  },

  recipesPage(state) {
    return state.recipesPage;
  },

  recipesTake(state) {
    return state.recipesTake;
  },

  recipesTotalCount(state) {
    return state.recipesTotalCount;
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
    return state.recentRecipes
      .map(id => state.recipes
        .filter(recipe => recipe.id === id)[0])
      .filter(recipe => recipe !== undefined);
  }
}