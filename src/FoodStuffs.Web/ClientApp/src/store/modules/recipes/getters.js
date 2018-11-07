import sortTypes from '../models/recipeSearchSortTypes';

export default {
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
