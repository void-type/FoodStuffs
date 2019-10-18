export default {
  setListResponse(context, data) {
    context.commit('SET_LIST_RESPONSE', data);
  },
  resetListRequest(context) {
    context.commit('RESET_LIST_REQUEST');
  },
  setListRequestPage(context, page) {
    context.commit('SET_LIST_REQUEST_PAGE', page);
  },
  setListRequestTake(context, take) {
    context.commit('SET_LIST_REQUEST_IS_PAGING_ENABLED', take !== null);
    context.commit('SET_LIST_REQUEST_TAKE', take);
  },
  setListRequestCategorySearch(context, categorySearch) {
    context.commit('SET_LIST_REQUEST_CATEGORY_SEARCH', categorySearch);
  },
  setListRequestNameSearch(context, nameSearch) {
    context.commit('SET_LIST_REQUEST_NAME_SEARCH', nameSearch);
  },
  setListRequestSort(context, sortName) {
    context.commit('SET_LIST_REQUEST_SORT', sortName);
  },
  addToRecent(context, recipe) {
    const recentRecipes = context.state.recent.slice();

    const indexOfCurrentInRecents = recentRecipes
      .map(recentRecipe => recentRecipe.id)
      .indexOf(recipe.id);

    const recipeListItem = {
      id: recipe.id,
      name: recipe.name,
    };

    if (indexOfCurrentInRecents > -1) {
      recentRecipes.splice(indexOfCurrentInRecents, 1);
    }
    if (recipe.id > 0) {
      recentRecipes.unshift(recipeListItem);
    }
    if (recentRecipes.length > 3) {
      recentRecipes.pop();
    }
    context.commit('SET_RECENT_RECIPES', recentRecipes);
  },
  removeFromRecent(context, id) {
    const recentRecipes = context.state.recent.slice();

    const indexOfCurrentInRecents = recentRecipes
      .map(recentRecipe => recentRecipe.id)
      .indexOf(id);

    if (indexOfCurrentInRecents > -1) {
      recentRecipes.splice(indexOfCurrentInRecents, 1);
    }

    context.commit('SET_RECENT_RECIPES', recentRecipes);
  },
};
