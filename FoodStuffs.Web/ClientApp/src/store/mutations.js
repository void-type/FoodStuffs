export default {
  setCurrentRecipe(state, recipe) {
    state.currentRecipe = recipe;
  },

  setCurrentRecipeName(state, value) {
    state.currentRecipe.name = value;
  },

  setCurrentRecipeIngredients(state, value) {
    state.currentRecipe.ingredients = value;
  },

  setCurrentRecipeDirections(state, value) {
    state.currentRecipe.directions = value;
  },

  setCurrentRecipePrepTimeMinutes(state, value) {
    state.currentRecipe.prepTimeMinutes = value;
  },

  setCurrentRecipeCookTimeMinutes(state, value) {
    state.currentRecipe.cookTimeMinutes = value;
  },

  addCategoryToCurrentRecipe(state, newCategoryName) {
    state.currentRecipe.categories.push(newCategoryName);
  },

  removeCategoryFromCurrentRecipe(state, indexOfCategoryToRemove) {
    state.currentRecipe.categories.splice(indexOfCategoryToRemove, 1);
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

  addCurrentRecipeToRecents(state) {
    if (state.recipesList.indexOf(state.currentRecipe) < 0) {
      return;
    }

    const recentRecipeIndex = state.recentRecipes
      .indexOf(state.recentRecipes
        .filter(recentRecipeId => recentRecipeId === state.currentRecipe.id)[0]);

    if (recentRecipeIndex > -1) {
      state.recentRecipes.splice(recentRecipeIndex, 1);
    }

    if (state.currentRecipe.id > 0) {
      state.recentRecipes.unshift(state.currentRecipe.id);
    }

    if (state.recentRecipes.length > 3) {
      state.recentRecipes.pop();
    }
  }
}