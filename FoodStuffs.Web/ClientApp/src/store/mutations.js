import valueFilters from "../models/valueFilters";

export default {
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
    recipe.prepTimeMinutes = valueFilters.limitIntegers(value);
  },

  setRecipeCookTimeMinutes(state, { recipe, value }) {
    recipe.cookTimeMinutes = valueFilters.limitIntegers(value);
  },

  addCategoryToRecipe(state, { recipe, categoryName }) {
    recipe.categories.push(categoryName);
  },

  removeCategoryFromRecipe(state, { recipe, categoryIndex }) {
    recipe.categories.splice(categoryIndex, 1);
  },

  setRecipesList(state, recipes) {
    state.recipesList = recipes;
  },

  setRecipesListPage(state, page) {
    state.recipesListPage = page;
  },

  setRecipesListTake(state, take) {
    state.recipesListTake = take;
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

  setIsError(state, status) {
    state.isError = status;
  },

  addRecipeToRecents(state, recipe) {
    // TODO: too much logic here?
    if (!state.recipesList.includes(recipe)) {
      return;
    }

    const recentRecipeIndex = state.recentRecipes.indexOf(recipe.id);

    if (recentRecipeIndex > -1) {
      state.recentRecipes.splice(recentRecipeIndex, 1);
    }

    if (recipe.id > 0) {
      state.recentRecipes.unshift(recipe.id);
    }

    if (state.recentRecipes.length > 3) {
      state.recentRecipes.pop();
    }
  }
}