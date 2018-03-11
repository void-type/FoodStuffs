import axios from "axios";

const methods = {
  listRecipes(params, success, failure) {
    axios.get("api/recipes/list", { params: params })
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  },

  createRecipe(recipe, success, failure) {
    axios.put("api/recipes", recipe)
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  },

  updateRecipe(recipe, success, failure) {
    axios.post("api/recipes", recipe)
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  },

  deleteRecipe(recipe, success, failure) {
    axios.delete("api/recipes", { params: { id: recipe.id } })
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  }
}

const callbacks = {
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

    if (postbackId){
      const id = postbackId.toString();
  
      const selectedRecipe = context.state.recipesList
        .filter(recipe => recipe.id.toString() === id)[0];

        context.commit("setCurrentRecipe", selectedRecipe);
    }
  }
}

export { methods, callbacks }