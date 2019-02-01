import trimAndCapitalize from '../../../filters/trimAndCapitalize';
import recipeApiModels from '../../../models/RecipeApiModels';
import webApi from '../../../webApi';

export default {
  fetchRecipesList(context) {
    webApi.recipes.list(
      context.state.recipesSearchParameters,
      (data) => {
        context.dispatch('setRecipesList', data);
      },
      response => context.dispatch('setApiFailureMessage', response),
    );
  },
  fetchRecipe(context, id) {
    webApi.recipes.get(
      id,
      (data) => {
        context.dispatch('setCurrentRecipe', data);
      },
      response => context.dispatch('setApiFailureMessage', response),
    );
  },
  saveRecipe(context, recipe) {
    webApi.recipes.save(
      recipe,
      (data) => {
        context.dispatch('fetchRecipe', data.id);
        context.dispatch('fetchRecipesList');
        context.dispatch('setSuccessMessage', data.message);
      },
      response => context.dispatch('setApiFailureMessage', response),
    );
  },
  deleteRecipe(context, recipe) {
    webApi.recipes.delete(
      recipe,
      (data) => {
        context.dispatch('setSuccessMessage', data.message);
        context.dispatch('fetchRecipesList');
      },
      response => context.dispatch('setApiFailureMessage', response),
    );
  },
  selectRecipe(context, recipe) {
    if (recipe && recipe.id !== undefined) {
      context.dispatch('fetchRecipe', recipe.id);
    } else {
      context.dispatch('setCurrentRecipe');
    }
  },
  setCurrentRecipe(context, recipe) {
    context.dispatch('addRecipeToRecents', context.getters.currentRecipe);
    context.commit('SET_CURRENT_RECIPE', recipe || new recipeApiModels.GetResponse());
  },
  setRecipeName(context, { recipe, value }) {
    context.commit('SET_RECIPE_NAME', { recipe, value });
  },
  setRecipeIngredients(context, { recipe, value }) {
    context.commit('SET_RECIPE_INGREDIENTS', { recipe, value });
  },
  setRecipeDirections(context, { recipe, value }) {
    context.commit('SET_RECIPE_DIRECTIONS', { recipe, value });
  },
  setRecipePrepTimeMinutes(context, { recipe, value }) {
    context.commit('SET_RECIPE_PREP_TIME_MINUTES', { recipe, value });
  },
  setRecipeCookTimeMinutes(context, { recipe, value }) {
    context.commit('SET_RECIPE_COOK_TIME_MINUTES', { recipe, value });
  },
  addCategoryToRecipe(context, { recipe, categoryName }) {
    const cleanedCategoryName = trimAndCapitalize(categoryName);

    const categoryDoesNotExist = recipe.categories
      .map(value => value.toUpperCase())
      .indexOf(categoryName.toUpperCase()) < 0;

    if (categoryDoesNotExist && cleanedCategoryName.length > 0) {
      context.commit('ADD_CATEGORY_TO_RECIPE', { recipe, cleanedCategoryName });
    }
  },
  removeCategoryFromRecipe(context, { recipe, categoryName }) {
    const categoryIndex = recipe.categories.indexOf(categoryName);

    if (categoryIndex > -1) {
      context.commit('REMOVE_CATEGORY_FROM_RECIPE', { recipe, categoryIndex });
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
    context.commit('SET_RECENT_RECIPES', recentRecipes);
  },
  setRecipesList(context, data) {
    context.commit('SET_RECIPES_LIST', data.items);
    context.commit('SET_RECIPES_LIST_TOTAL_COUNT', data.totalCount);
  },
  setRecipesSearchParametersNameSearch(context, nameSearch) {
    context.commit('SET_RECIPES_SEARCH_PARAMETERS_NAME_SEARCH', nameSearch);
  },
  setRecipesSearchParametersCategorySearch(context, categorySearch) {
    context.commit('SET_RECIPES_SEARCH_PARAMETERS_CATEGORY_SEARCH', categorySearch);
  },
  setRecipesSearchParametersPage(context, page) {
    context.commit('SET_RECIPES_SEARCH_PARAMETERS_PAGE', page);
  },
  setRecipesSearchParametersTake(context, take) {
    context.commit('SET_RECIPES_SEARCH_PARAMETERS_TAKE', take);
  },
  cycleSelectedNameSortType(context) {
    const sortTypes = recipeApiModels.GetRequestSortTypes;

    const currentSortId = sortTypes
      .indexOf(sortTypes
        .filter(type => type.name === context.state.recipesSearchParameters.sort)[0]);

    const newSortId = (currentSortId + 1) % sortTypes.length;
    const newSortName = sortTypes[newSortId].name;

    context.commit('SET_RECIPES_SEARCH_PARAMETERS_SORT', newSortName);
    context.dispatch('fetchRecipesList');
  },
};
