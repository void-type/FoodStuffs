import webApi from "./webApi"
import Recipe from "../models/recipe"

const defaultCallbacks = {
  onSuccess(context, data) {
    context.commit("setMessage", data.message);
  },

  onSuccessWithRefresh(context, data) {
    context.dispatch("fetchRecipes");
    context.dispatch("selectNewRecipe");
    this.onSuccess(context, data);
  },

  onFailure(context, response) {
    context.commit("setIsError");

    if (response.status >= 500) {
      // server error
      context.commit("setMessage", response.data.message);
    } else {
      // validation errors
      context.commit("setFieldsInError", response.data.items.map((item) => item.fieldName));
      context.commit("setMessages", response.data.items.map((item) => item.errorMessage));
    }
  }
}

export default {
  // Api Calls
  fetchRecipes(context) {
    webApi.listRecipes(
      data => context.commit("setRecipesList", data.items),
      response => defaultCallbacks.onFailure(context, response));
  },

  saveRecipe(context, recipe) {
    context.commit("clearErrors");

    // If the recipe is new, create it. Otherwise, update it.
    if (recipe.id === undefined || recipe.id < 1) {
      webApi.createRecipe(
        recipe,
        data => defaultCallbacks.onSuccessWithRefresh(context, data),
        response => defaultCallbacks.onFailure(context, response));
    } else {
      webApi.updateRecipe(
        recipe,
        data => defaultCallbacks.onSuccessWithRefresh(context, data),
        response => defaultCallbacks.onFailure(context, response));
    }
  },

  deleteRecipe(context, recipe) {
    context.commit("clearErrors");
    if (confirm("Are you sure you want to delete this recipe?")) {
      webApi.deleteRecipe(
        recipe,
        data => defaultCallbacks.onSuccessWithRefresh(context, data),
        response => defaultCallbacks.onFailure(context, response));
    }
  },

  selectRecipe(context, recipe) {
    context.commit("clearErrors");
    context.commit("selectRecipe", recipe);
  },

  selectNewRecipe(context) {
    context.commit("clearErrors");
    context.commit("selectRecipe", new Recipe());
  }
}