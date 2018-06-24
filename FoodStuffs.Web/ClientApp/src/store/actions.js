import trimAndCapitalize from '../filters/trimAndCapitalize';
import Recipe from '../models/recipe';
import applicationInfoApi from '../models/applicationInfoApi';
import recipeApi from '../models/recipeApi';
import sortTypes from '../models/recipeSearchSortTypes';
import webApi from '../models/webApi';

export default {
  clearMessages(context) {
    context.commit('setMessageIsError', false);
    context.commit('setFieldsInError', []);
    context.commit('setMessages', []);
  },

  fetchApplicationInfo(context) {
    applicationInfoApi.fetchApplicationInfo(
      (data) => {
        context.commit('setApplicationName', data.applicationName);
        context.commit('setUserName', data.userName);
        webApi.setRequestVerificationToken(data.antiforgeryToken);
        webApi.setTitle(data.applicationName);
      },
      (response) => {
        webApi.onFailure(context, response);
      },
    );
  },

  fetchRecipe(context, id) {
    recipeApi.getRecipe(
      id,
      (data) => {
        context.dispatch('setCurrentRecipe', data);
      },
      response => webApi.onFailure(context, response),
    );
  },

  fetchRecipesList(context) {
    recipeApi.listRecipes(
      context.state.recipesSearchParameters,
      (data) => {
        context.dispatch('setRecipesList', data);
      },
      response => webApi.onFailure(context, response),
    );
  },

  deleteRecipe(context, recipe) {
    context.dispatch('clearMessages');

    recipeApi.deleteRecipe(
      recipe,
      (data) => {
        webApi.onSuccess(context, data);
        context.dispatch('fetchRecipesList');
      },
      response => webApi.onFailure(context, response),
    );
  },

  saveRecipe(context, recipe) {
    context.dispatch('clearMessages');

    if (recipe.id === undefined || recipe.id < 1) {
      recipeApi.createRecipe(
        recipe,
        (data) => {
          context.dispatch('fetchRecipe', data.id);
          context.dispatch('fetchRecipesList');
          webApi.onSuccess(context, data);
        },
        response => webApi.onFailure(context, response),
      );
    } else {
      recipeApi.updateRecipe(
        recipe,
        (data) => {
          context.dispatch('fetchRecipe', data.id);
          context.dispatch('fetchRecipesList');
          webApi.onSuccess(context, data);
        },
        response => webApi.onFailure(context, response),
      );
    }
  },

  selectRecipe(context, recipe) {
    context.dispatch('clearMessages');

    if (recipe && recipe.id !== undefined) {
      context.dispatch('fetchRecipe', recipe.id);
    } else {
      context.dispatch('setCurrentRecipe');
    }
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
    context.commit('setRecipePrepTimeMinutes', { recipe, value });
  },

  setRecipeCookTimeMinutes(context, { recipe, value }) {
    context.commit('setRecipeCookTimeMinutes', { recipe, value });
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

  addRecipeToRecents(context, recipe) {
    const recentRecipes = context.state.recentRecipes.slice();

    const indexOfCurrentInRecents = recentRecipes
      .map(recentRecipe => recentRecipe.id)
      .indexOf(recipe.id);

    const recipeListItem = {
      id: recipe.id,
      name: recipe.name,
      categories: recipe.categories,
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
    context.commit('setRecentRecipes', recentRecipes);
  },

  setRecipesList(context, data) {
    context.commit('setRecipesList', data.items);
    context.commit('setRecipesListTotalCount', data.totalCount);
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

  setRecipesSearchParametersTake(context, take) {
    context.commit('setRecipesSearchParametersTake', take);
  },

  cycleSelectedNameSortType(context) {
    const currentSortId = sortTypes
      .indexOf(sortTypes
        .filter(type => type.name === context.state.recipesSearchParameters.sort)[0]);
    const newSortId = (currentSortId + 1) % sortTypes.length;
    const newSortName = sortTypes[newSortId].name;
    context.commit('setRecipesSearchParametersSort', newSortName);
    context.dispatch('fetchRecipesList');
  },
};
