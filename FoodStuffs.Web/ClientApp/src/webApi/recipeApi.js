import axios from 'axios';

export default {
  delete(recipe, success, failure) {
    axios.delete('api/recipes', { params: { id: recipe.id } })
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  },

  get(id, success, failure) {
    axios.get('api/recipes', { params: { id } })
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  },

  list(recipeSearchParameters, success, failure) {
    axios.get('api/recipes/list', { params: recipeSearchParameters })
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  },

  save(recipe, success, failure) {
    axios.post('api/recipes', recipe)
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  },
};

