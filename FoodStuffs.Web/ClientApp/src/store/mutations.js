/* eslint-disable no-param-reassign */
export default {
  setApplicationName(state, appName) {
    state.applicationName = appName;
  },

  setUserName(state, userName) {
    state.userName = userName;
  },

  setCurrentRecipe(state, recipe) {
    state.currentRecipe = recipe;
  },

  setRecipeName(state, { recipe, value }) {
    recipe.name = value;
  },

  setRecipeIngredients(state, { recipe, value }) {
    recipe.ingredients = value;
  },

  setRecipeDirections(state, { recipe, value }) {
    recipe.directions = value;
  },

  setRecipePrepTimeMinutes(state, { recipe, value }) {
    recipe.prepTimeMinutes = value;
  },

  setRecipeCookTimeMinutes(state, { recipe, value }) {
    recipe.cookTimeMinutes = value;
  },

  addCategoryToRecipe(state, { recipe, cleanedCategoryName }) {
    recipe.categories.push(cleanedCategoryName);
  },

  removeCategoryFromRecipe(state, { recipe, categoryIndex }) {
    recipe.categories.splice(categoryIndex, 1);
  },

  setRecentRecipes(state, recentRecipes) {
    state.recentRecipes = recentRecipes;
  },

  setRecipesList(state, recipes) {
    state.recipesList = recipes;
  },

  setRecipesListTotalCount(state, totalCount) {
    state.recipesListTotalCount = totalCount;
  },

  setRecipesSearchParametersNameSearch(state, nameSearch) {
    state.recipesSearchParameters.nameSearch = nameSearch;
  },

  setRecipesSearchParametersCategorySearch(state, categorySearch) {
    state.recipesSearchParameters.categorySearch = categorySearch;
  },

  setRecipesSearchParametersPage(state, page) {
    state.recipesSearchParameters.page = page;
  },

  setRecipesSearchParametersTake(state, take) {
    state.recipesSearchParameters.take = take;
  },

  setRecipesSearchParametersSort(state, sort) {
    state.recipesSearchParameters.sort = sort;
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

  setMessageIsError(state, status) {
    state.messageIsError = status;
  },
};
/* eslint-enable no-param-reassign */
