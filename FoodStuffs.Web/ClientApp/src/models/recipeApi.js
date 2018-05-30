import axios from 'axios';

export default {
  createRecipe(recipe, success, failure) {
    axios.put('api/recipes', recipe)
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  },

  listRecipes(params, success, failure) {
    axios.get('api/recipes/list', { params })
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  },

  deleteRecipe(recipe, success, failure) {
    axios.delete('api/recipes', { params: { id: recipe.id } })
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  },

  updateRecipe(recipe, success, failure) {
    axios.post('api/recipes', recipe)
      .then(response => success(response.data))
      .catch(error => failure(error.response));
  },
};

function apiConfigure() {
  const csrfToken = document.getElementById('requestVerificationToken').value;

  const headers = {
    'X-CSRF-TOKEN': csrfToken,
    'X-Requested-With': 'XMLHttpRequest',
  };

  axios.defaults.headers.post = headers;
  axios.defaults.headers.put = headers;
  axios.defaults.headers.delete = headers;
}

apiConfigure();
