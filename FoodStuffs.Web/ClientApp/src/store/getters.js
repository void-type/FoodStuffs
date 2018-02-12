export default {
  currentRecipe(state) {
    return state.currentRecipe;
  },

  messages(state) {
    return state.messages;
  },

  fieldsInError(state) {
    return state.fieldsInError;
  },

  isFieldInError: (state) => (fieldName) => {
    return state.fieldsInError.indexOf(fieldName) > -1;
  },

  isError(state) {
    return state.isError;
  },

  recipesList(state) {
    return state.recipesList;
  },

  recipesListPage(state) {
    return state.recipesListPage;
  },

  recipesListTake(state) {
    return state.recipesListTake;
  },

  recipesListTotalCount(state) {
    return state.recipesListTotalCount;
  },

  recentRecipes(state) {
    return state.recentRecipes
      .map(id => state.recipesList
        .filter(recipe => recipe.id === id)[0])
      .filter(recipe => recipe !== undefined);
  },

  recipesSearchParameters(state) {
    return state.recipesSearchParameters;
  }
}