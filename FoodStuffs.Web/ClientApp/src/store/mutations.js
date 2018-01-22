export default {
  clearErrors(state) {
    state.isError = false;
    state.fieldsInError = [];
    state.messages = [];
  },

  selectRecipe(state, recipe) {
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