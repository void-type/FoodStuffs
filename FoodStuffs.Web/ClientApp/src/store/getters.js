import sortTypes from "../models/recipeSearchSortTypes";

export default {
  applicationName(state) {
    return state.applicationName;
  },

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

  findRecipeById: (state) => (idToFind) => {
    return state.recipesList.filter(item => item.id === idToFind)[0];
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

  recentRecipes(state, getters) {
    return state.recentRecipeIds.map(id => getters.findRecipeById(id));
  },

  recipesSearchParameters(state) {
    return state.recipesSearchParameters;
  },

  recipesSearchParametersSortType(state) {
    return sortTypes.filter(type => type.name === state.recipesSearchParameters.sort)[0];
  }
}