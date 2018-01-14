import Recipe from "../models/recipe.js"

const commonFunctions =
  {
    clearErrors(state) {
      state.isError = false;
      state.fieldsInError = [];
      state.messages = [];
    }
  }

export default {
  clearErrors(state) {
    commonFunctions.clearErrors(state);
  },

  selectRecipe(state, recipe) {
    commonFunctions.clearErrors(state);
    state.currentRecipe = recipe;
  },

  selectNewRecipe(state) {
    commonFunctions.clearErrors(state);
    state.currentRecipe = new Recipe();
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
  }
}
