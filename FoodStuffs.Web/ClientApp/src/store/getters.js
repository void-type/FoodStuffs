import sortTypes from '../models/recipeSearchSortTypes';

export default {
  applicationName(state) {
    return state.applicationName;
  },

  userName(state) {
    return state.userName;
  },

  messageIsError(state) {
    return state.messageIsError;
  },

  isFieldInError: state => fieldName => state.fieldsInError.indexOf(fieldName) > -1,

  messages(state) {
    return state.messages;
  },

  currentRecipe(state) {
    return state.currentRecipe;
  },

  recentRecipes(state) {
    return state.recentRecipes;
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
