import { methods, callbacks } from "../models/webApi";
import Recipe from "../models/recipe";

export default {
  fetchRecipes(context, postbackId) {
    methods.listRecipes(
      context.state.recipesSearchParameters,
      data => callbacks.onFetchListSuccess(context, data, postbackId),
      response => callbacks.onFailure(context, response));
  },

  saveRecipe(context, recipe) {
    context.dispatch("clearMessages");

    if (recipe.id === undefined || recipe.id < 1) {
      methods.createRecipe(
        recipe,
        data => callbacks.onSuccess(context, data),
        response => callbacks.onFailure(context, response));
    } else {
      methods.updateRecipe(
        recipe,
        data => callbacks.onSuccess(context, data),
        response => callbacks.onFailure(context, response));
    }
  },

  deleteRecipe(context, recipe) {
    context.dispatch("clearMessages");
    if (confirm("Are you sure you want to delete this recipe?")) {
      methods.deleteRecipe(
        recipe,
        data => callbacks.onSuccess(context, data),
        response => callbacks.onFailure(context, response));
    }
  },

  selectRecipe(context, recipe) {
    context.dispatch("clearMessages");
    context.commit("addCurrentRecipeToRecents");
    context.commit("setCurrentRecipe", recipe || new Recipe());
  },

  addCategoryToCurrentRecipe(context, newCategoryName) {
    newCategoryName = newCategoryName
      .trim()
      .split(" ")
      .filter(word => word.length > 0)
      .map(word => word[0].toUpperCase() + word.substring(1))
      .join(" ");

    const categoryDoesNotExist = context.state.currentRecipe.categories
      .map((value) => value.toUpperCase())
      .indexOf(newCategoryName.toUpperCase()) < 0;

    if (categoryDoesNotExist && newCategoryName.length > 0) {
      context.commit("addCategoryToCurrentRecipe", newCategoryName);
    }
  },

  removeCategoryFromCurrentRecipe(context, categoryToRemove) {
    const index = context.state.currentRecipe.categories.indexOf(categoryToRemove);

    if (index > -1) {
      context.commit("removeCategoryFromCurrentRecipe", index);
    }
  },

  clearMessages(context) {
    context.commit("setIsError", false);
    context.commit("setFieldsInError", []);
    context.commit("setMessages", []);
  }
}