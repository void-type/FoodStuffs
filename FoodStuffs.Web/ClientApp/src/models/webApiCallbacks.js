export default {
  onSuccess(context, data) {
    context.dispatch('fetchRecipes', data.id);
    context.commit('setIsError', false);
    context.commit('setMessage', data.message);
  },

  onFailure(context, response) {
    context.commit('setIsError', true);
    if (response === undefined || response === null) {
      context.commit('setMessage', 'Cannot connect to server.');
    } else if (response.status >= 500) {
      context.commit('setMessage', response.data.message);
    } else {
      context.commit('setMessages', response.data.items.map(item => item.errorMessage));
      context.commit('setFieldsInError', response.data.items.map(item => item.fieldName));
    }
  },

  onFetchListSuccess(context, data, postbackId) {
    context.dispatch('setRecipesList', data);
    if (postbackId > 0) {
      const selectedRecipe = context.getters.findRecipeById(postbackId);
      context.dispatch('setCurrentRecipe', selectedRecipe);
    }
  },
};
