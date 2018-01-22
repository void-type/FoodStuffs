export default {
  clearErrors(state) {
    state.isError = false;
    state.fieldsInError = [];
    state.messages = [];
  },

  selectRecipe(state, recipe) {
    const recipeIsRecent = state.recentRecipes.indexOf(state.currentRecipe);

    if (recipeIsRecent > -1) {
      state.recentRecipes.splice(recipeIsRecent, 1);
    }

    if (state.currentRecipe.id > 0) {
      state.recentRecipes.unshift(state.currentRecipe);
    }

    if (state.recentRecipes.length > 3) {
      state.recentRecipes.pop();
    }

      state.currentRecipe = recipe;
  },

  setRecipesList(state, recipes) {
    state.recipes = recipes;
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
    state.isError = status || true;
  },

  addCategoryToCurrentRecipe(state, newCategoryName) {
    const categories = state.currentRecipe.categories;
    const trimmedName = newCategoryName.trim();
    const categoryDoesNotExist = categories.indexOf(trimmedName) < 0;


    if (categoryDoesNotExist && trimmedName !== "") {
      categories.push(trimmedName);
    }
  },

  removeCategoryFromCurrentRecipe(state, categoryToRemove) {
    const categories = state.currentRecipe.categories;
    const index = categories.indexOf(categoryToRemove);

    if (index > -1) {
      categories.splice(index, 1);
    }
  }


}