import webApi from "./webApi"
import Recipe from "../models/recipe"

const defaultCallbacks = {
  onSuccess(context, data) {
    context.dispatch("fetchRecipes", data.id);
    context.commit("setIsError", false);
    context.commit("setMessage", data.message);
  },

  onFailure(context, response) {
    context.commit("setIsError", true);

    if (response === undefined || response === null) {
      context.commit("setMessage", "Cannot connect to server.");
    } else if (response.status >= 500) {
      context.commit("setMessage", response.data.message);
    } else {
      context.commit("setMessages", response.data.items.map((item) => item.errorMessage));
      context.commit("setFieldsInError", response.data.items.map((item) => item.fieldName));
    }
  }
}

export default {
  fetchRecipes(context, postbackId) {
    webApi.listRecipes(
      function (data) {
        context.commit("setRecipesList", data.items);

        const id = (postbackId) ? postbackId.toString() : null;

        const selectedRecipe = context.state.recipes
          .filter(recipe => recipe.id.toString() === id)[0]
          || new Recipe();

        context.commit("setCurrentRecipe", selectedRecipe);
      },
      response => defaultCallbacks.onFailure(context, response));
  },

  saveRecipe(context, recipe) {
    context.dispatch("clearErrors");

    if (recipe.id === undefined || recipe.id < 1) {
      webApi.createRecipe(
        recipe,
        data => defaultCallbacks.onSuccess(context, data),
        response => defaultCallbacks.onFailure(context, response));
    } else {
      webApi.updateRecipe(
        recipe,
        data => defaultCallbacks.onSuccess(context, data),
        response => defaultCallbacks.onFailure(context, response));
    }
  },

  deleteRecipe(context, recipe) {
    context.dispatch("clearErrors");
    if (confirm("Are you sure you want to delete this recipe?")) {
      webApi.deleteRecipe(
        recipe,
        data => defaultCallbacks.onSuccess(context, data),
        response => defaultCallbacks.onFailure(context, response));
    }
  },

  selectRecipe(context, recipe) {
    context.dispatch("clearErrors");
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

  clearErrors(context) {
    context.commit("setIsError", false);
    context.commit("setFieldsInError", []);
    context.commit("setMessages", []);
  }
}