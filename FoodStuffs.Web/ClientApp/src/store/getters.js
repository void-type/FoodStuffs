import sortTypes from '../models/recipeSearchSortTypes';

export default {
  applicationName(state) {
    return state.applicationName;
  },

  userName(state) {
    return state.userName;
  },

  currentRecipe(state) {
    return state.currentRecipe;
  },

  findRecipeById: state => idToFind => state.recipesList
    .filter(item => item.id === parseInt(idToFind, 10))[0] || null,

  messageIsError(state) {
    return state.messageIsError;
  },

  isFieldInError: state => fieldName => state.fieldsInError.indexOf(fieldName) > -1,

  messages(state) {
    return state.messages;
  },

  recentRecipes(state, getters) {
    const recents = state.recentRecipeIds
      .map(id => getters.findRecipeById(id))
      .filter(recipe => recipe != null);
    return recents;
  },

  recipesList(state) {
    return state.recipesList;
  },

  recipesListTotalCount(state) {
    return state.recipesListTotalCount;
  },

  recipesSearchParameters(state) {
    return state.recipesSearchParameters;
  },

  recipesSearchParametersSortType(state) {
    return sortTypes.filter(type => type.name === state.recipesSearchParameters.sort)[0];
  },
};
