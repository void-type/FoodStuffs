const webApiCallbacks = {
  onSuccess(context, data) {
    context.dispatch("fetchRecipes", data.id || null);
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
  },

  onFetchListSuccess(context, data, postbackId) {
    context.commit("setRecipesList", data.items);
    context.commit("setRecipesListTotalCount", data.totalCount);
    context.commit("setRecipesListPage", data.page);
    context.commit("setRecipesListTake", data.take);

    if (postbackId > 0) {
      const selectedRecipe = context.getters.findRecipeById(recipe.id);
      context.commit("setCurrentRecipe", selectedRecipe);
    }
  }
}

export { webApiCallbacks }