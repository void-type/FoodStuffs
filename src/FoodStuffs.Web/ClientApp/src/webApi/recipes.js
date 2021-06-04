import axios from 'axios';

export default {
  list(params, success, failure) {
    axios.get('/api/recipes', { params })
      .then((response) => success(response.data))
      .catch((error) => failure(error.response));
  },
  get(id, success, failure) {
    axios.get(`/api/recipes/${id}`)
      .then((response) => success(response.data))
      .catch((error) => failure(error.response));
  },
  save(recipe, success, failure) {
    axios.post('/api/recipes', recipe)
      .then((response) => success(response.data))
      .catch((error) => failure(error.response));
  },
  delete(id, success, failure) {
    axios.delete(`/api/recipes/${id}`, { params: { id } })
      .then((response) => success(response.data))
      .catch((error) => failure(error.response));
  },
};
