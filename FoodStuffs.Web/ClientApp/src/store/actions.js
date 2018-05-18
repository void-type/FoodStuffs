import limitIntegers from '../filters/limitIntegers';
import trimAndCapitalize from '../filters/trimAndCapitalize';
import applicationNameApi from '../models/applicationNameApi';
import Recipe from '../models/recipe';
import recipeApi from '../models/recipeApi';
import sortTypes from '../models/recipeSearchSortTypes';
import webApiCallbacks from '../models/webApiCallbacks';

export default {
  fetchApplicationName(context) {
    context.commit('setApplicationName', applicationNameApi.applicationName);
  },

  fetchRecipes(context, postbackId) {
    recipeApi.listRecipes(
      context.state.recipesSearchParameters,
      data => webApiCallbacks.onFetchListSuccess(context, data, postbackId),
      response => webApiCallbacks.onFailure(context, response),
    );
  },

  setRecipesList(context, data) {
    context.commit('setRecipesList', data.items);
    context.commit('setRecipesListTotalCount', data.totalCount);
    context.commit('setRecipesListPage', data.page);
    context.commit('setRecipesListTake', data.take);
  },

  saveRecipe(context, recipe) {
    context.dispatch('clearMessages');

    if (recipe.id === undefined || recipe.id < 1) {
      recipeApi.createRecipe(
        recipe,
        data => webApiCallbacks.onSuccess(context, data),
        response => webApiCallbacks.onFailure(context, response),
      );
    } else {
      recipeApi.updateRecipe(
        recipe,
        data => webApiCallbacks.onSuccess(context, data),
        response => webApiCallbacks.onFailure(context, response),
      );
    }
  },

  deleteRecipe(context, recipe) {
    context.dispatch('clearMessages');
    recipeApi.deleteRecipe(
      recipe,
      data => webApiCallbacks.onSuccess(context, data),
      response => webApiCallbacks.onFailure(context, response),
    );
  },

  selectRecipe(context, recipe) {
    context.dispatch('clearMessages');
    context.dispatch('setCurrentRecipe', recipe);
  },

  setCurrentRecipe(context, recipe) {
    context.dispatch('addRecipeToRecents', context.getters.currentRecipe);
    context.commit('setCurrentRecipe', recipe || new Recipe());
  },

  setRecipeName(context, { recipe, value }) {
    context.commit('setRecipeName', { recipe, value });
  },

  setRecipeIngredients(context, { recipe, value }) {
    context.commit('setRecipeIngredients', { recipe, value });
  },

  setRecipeDirections(context, { recipe, value }) {
    context.commit('setRecipeDirections', { recipe, value });
  },

  setRecipePrepTimeMinutes(context, { recipe, value }) {
    const limitedValue = limitIntegers(value);
    context.commit('setRecipePrepTimeMinutes', { recipe, limitedValue });
  },

  setRecipeCookTimeMinutes(context, { recipe, value }) {
    const limitedValue = limitIntegers(value);
    context.commit('setRecipeCookTimeMinutes', { recipe, limitedValue });
  },

  addCategoryToRecipe(context, { recipe, categoryName }) {
    const cleanedCategoryName = trimAndCapitalize(categoryName);

    const categoryDoesNotExist = recipe.categories
      .map(value => value.toUpperCase())
      .indexOf(categoryName.toUpperCase()) < 0;

    if (categoryDoesNotExist && cleanedCategoryName.length > 0) {
      context.commit('addCategoryToRecipe', { recipe, cleanedCategoryName });
    }
  },

  removeCategoryFromRecipe(context, { recipe, categoryName }) {
    const categoryIndex = recipe.categories.indexOf(categoryName);

    if (categoryIndex > -1) {
      context.commit('removeCategoryFromRecipe', { recipe, categoryIndex });
    }
  },

  clearMessages(context) {
    context.commit('setIsError', false);
    context.commit('setFieldsInError', []);
    context.commit('setMessages', []);
  },

  setRecipesSearchParametersNameSearch(context, nameSearch) {
    context.commit('setRecipesSearchParametersNameSearch', nameSearch);
  },

  setRecipesSearchParametersCategorySearch(context, categorySearch) {
    context.commit('setRecipesSearchParametersCategorySearch', categorySearch);
  },

  setRecipesSearchParametersPage(context, page) {
    context.commit('setRecipesSearchParametersPage', page);
  },

  cycleSelectedNameSortType(context) {
    const currentSortId = sortTypes
      .indexOf(sortTypes
        .filter(type => type.name === context.state.recipesSearchParameters.sort)[0]);
    const newSortId = (currentSortId + 1) % sortTypes.length;
    const newSortName = sortTypes[newSortId].name;
    context.commit('setRecipesSearchParametersSort', newSortName);
    context.dispatch('fetchRecipes');
  },

  addRecipeToRecents(context, recipe) {
    if (!context.getters.recipesList.includes(recipe)) {
      return;
    }

    const recentRecipeIds = context.state.recentRecipeIds.slice();
    const recentRecipeIndex = recentRecipeIds.indexOf(recipe.id);

    if (recentRecipeIndex > -1) {
      recentRecipeIds.splice(recentRecipeIndex, 1);
    }
    if (recipe.id > 0) {
      recentRecipeIds.unshift(recipe.id);
    }
    if (recentRecipeIds.length > 3) {
      recentRecipeIds.pop();
    }
    context.commit('setRecentRecipeIds', recentRecipeIds);
  },
};
