import sortTypes from '../models/recipeSearchSortTypes';

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

  isFieldInError: state => fieldName => state.fieldsInError.indexOf(fieldName) > -1,

  isError(state) {
    return state.isError;
  },

  recipesList(state) {
    return state.recipesList;
  },

  findRecipeById: state => idToFind => state.recipesList
    .filter(item => item.id === parseInt(idToFind, 10))[0] || null,

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
    const recents = state.recentRecipeIds
      .map(id => getters.findRecipeById(id))
      .filter(recipe => recipe != null);
    return recents;
  },

  recipesSearchParameters(state) {
    return state.recipesSearchParameters;
  },

  recipesSearchParametersSortType(state) {
    return sortTypes.filter(type => type.name === state.recipesSearchParameters.sort)[0];
  },
};
