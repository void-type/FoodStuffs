export default {
  setListResponse(context, data) {
    context.commit('SET_LIST_RESPONSE', data);
  },
  setListRequest(context, request) {
    context.commit('SET_LIST_REQUEST', request);
  },
  addToRecent(context, recipe) {
    const limit = 3;

    if (recipe === null) {
      return;
    }

    const recentRecipes = context.state.recentRecipes.slice();

    const indexOfCurrentInRecents = recentRecipes
      .map((recentRecipe) => recentRecipe.id)
      .indexOf(recipe.id);

    // TODO: update these recipes if edited, these can get out of sync
    const recipeListItem = {
      ...recipe,
    };

    if (indexOfCurrentInRecents > -1) {
      recentRecipes.splice(indexOfCurrentInRecents, 1);
    }

    if (recipe.id > 0) {
      recentRecipes.unshift(recipeListItem);
    }

    const limitedRecipes = recentRecipes.slice(0, limit);

    context.commit('SET_RECENT_RECIPES', limitedRecipes);
  },
  removeFromRecent(context, id) {
    const recentRecipes = context.state.recentRecipes.slice();

    const indexOfCurrentInRecents = recentRecipes
      .map((recentRecipe) => recentRecipe.id)
      .indexOf(id);

    if (indexOfCurrentInRecents > -1) {
      recentRecipes.splice(indexOfCurrentInRecents, 1);
    }

    context.commit('SET_RECENT_RECIPES', recentRecipes);
  },
};
