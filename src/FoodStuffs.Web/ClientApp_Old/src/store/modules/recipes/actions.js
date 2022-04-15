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
  updateRecent(context, recipe) {
    if (recipe === null) {
      return;
    }

    const recentRecipes = context.state.recentRecipes.slice();

    const indexOfCurrentInRecents = recentRecipes
      .map((recentRecipe) => recentRecipe.id)
      .indexOf(recipe.id);

    if (indexOfCurrentInRecents < 0) {
      return;
    }

    const recipeListItem = {
      ...recipe,
    };

    recentRecipes[indexOfCurrentInRecents] = recipeListItem;

    context.commit('SET_RECENT_RECIPES', recentRecipes);
  },
};
