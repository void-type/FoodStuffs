import { webApiCallbacks } from "./actions.helpers";
import valueFilters from "../models/valueFilters";
import webApi from "../models/webApi";
import Recipe from "../models/recipe";
import sortTypes from "../models/recipeSearchSortTypes";

export default {
  fetchRecipes(context, postbackId) {
    webApi.listRecipes(
      context.state.recipesSearchParameters,
      data => webApiCallbacks.onFetchListSuccess(context, data, postbackId),
      response => webApiCallbacks.onFailure(context, response));
  },

  setRecipesList(context, data) {
    context.commit("setRecipesList", data.items);
    context.commit("setRecipesListTotalCount", data.totalCount);
    context.commit("setRecipesListPage", data.page);
    context.commit("setRecipesListTake", data.take);
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
    context.dispatch("addRecipeToRecents", recipe);
    context.dispatch("setCurrentRecipe", recipe);
  },

  setCurrentRecipe(context, recipe) {
    context.commit("setCurrentRecipe", recipe || new Recipe());
  },

  addCategoryToRecipe(context, { recipe, categoryName }) {
    categoryName = valueFilters.trimAndCapitalize(categoryName);

    const categoryDoesNotExist = recipe.categories
      .map((value) => value.toUpperCase())
      .indexOf(categoryName.toUpperCase()) < 0;

    if (categoryDoesNotExist && categoryName.length > 0) {
      context.commit("addCategoryToRecipe", { recipe, categoryName });
    }
  },

  removeCategoryFromRecipe(context, { recipe, categoryName }) {
    const categoryIndex = recipe.categories.indexOf(categoryName);

    if (categoryIndex > -1) {
      context.commit("removeCategoryFromRecipe", { recipe, categoryIndex });
    }
  },

  clearMessages(context) {
    context.commit("setIsError", false);
    context.commit("setFieldsInError", []);
    context.commit("setMessages", []);
  },

  cycleSelectedNameSortType(context) {
    const currentSortId = sortTypes.indexOf(
      sortTypes.filter(type => type.name === context.state.recipesSearchParameters.sort)[0]
    );
    const newSortId = (currentSortId + 1) % sortTypes.length;
    const newSortName = sortTypes[newSortId].name;
    context.commit("setRecipesSearchParametersSort", newSortName);
    context.dispatch("fetchRecipes");
  },

  addRecipeToRecents(context, recipe) {
    if (!context.getters.recipesList.includes(recipe)) {
      return;
    }

    let recentRecipeIds = context.state.recentRecipeIds.slice();
    const recentRecipeIndex = recentRecipeIds.indexOf(recipe.id);

    if (recentRecipeIndex > -1) {
      recentRecipeIds.splice(recentRecipeIndex, 1);
    }
    if (recipe.id > 0) {
      recentRecipeIds.unshift(recipe.id);
    }
    if (recentRecipeIds.length > 3) {
      recentRecipeIds.pop();
    }
    context.commit("setRecentRecipeIds", recentRecipeIds)
  }
}