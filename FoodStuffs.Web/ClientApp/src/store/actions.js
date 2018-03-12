import { webApiCallbacks } from "./actions.helpers";
import valueFilters from "../models/valueFilters";
import webApi from "../models/webApi";
import Recipe from "../models/recipe";

export default {
  fetchRecipes(context, postbackId) {
    webApi.listRecipes(
      context.state.recipesSearchParameters,
      data => webApiCallbacks.onFetchListSuccess(context, data, postbackId),
      response => webApiCallbacks.onFailure(context, response));
  },

  saveRecipe(context, recipe) {
    context.dispatch("clearMessages");

    if (recipe.id === undefined || recipe.id < 1) {
      webApi.createRecipe(
        recipe,
        data => webApiCallbacks.onSuccess(context, data),
        response => webApiCallbacks.onFailure(context, response));
    } else {
      webApi.updateRecipe(
        recipe,
        data => webApiCallbacks.onSuccess(context, data),
        response => webApiCallbacks.onFailure(context, response));
    }
  },

  deleteRecipe(context, recipe) {
    context.dispatch("clearMessages");
    if (confirm("Are you sure you want to delete this recipe?")) {
      webApi.deleteRecipe(
        recipe,
        data => webApiCallbacks.onSuccess(context, data),
        response => webApiCallbacks.onFailure(context, response));
    }
  },

  selectRecipe(context, recipe) {
    context.dispatch("clearMessages");
    context.commit("addRecipeToRecents", recipe);
    context.commit("setCurrentRecipe", recipe || new Recipe());
  },

  addCategoryToRecipe(context, { recipe, categoryName }) {
    categoryName = valueFilters.trimAndCapitalize(categoryName);

    const categoryDoesNotExist = recipe.categories
      .map((value) => value.toUpperCase())
      .indexOf(categoryName.toUpperCase()) < 0;

    if (categoryDoesNotExist && categoryName.length > 0) {
      context.commit("addCategoryToRecipe", {recipe, categoryName});
    }
  },

  removeCategoryFromRecipe(context, { recipe, categoryName }) {
    const categoryIndex = recipe.categories.indexOf(categoryName);

    if (categoryIndex > -1) {
      context.commit("removeCategoryFromRecipe", {recipe, categoryIndex});
    }
  },

  clearMessages(context) {
    context.commit("setIsError", false);
    context.commit("setFieldsInError", []);
    context.commit("setMessages", []);
  }
}