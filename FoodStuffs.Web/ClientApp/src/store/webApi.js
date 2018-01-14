import axios from "axios";

export default {
  listRecipes(success, failure) {
    axios.get("api/recipes/list")
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
