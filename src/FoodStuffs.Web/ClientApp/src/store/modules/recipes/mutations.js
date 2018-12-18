/* eslint-disable no-param-reassign */
export default {
  SET_CURRENT_RECIPE(state, recipe) {
    state.currentRecipe = recipe;
  },
  SET_RECIPE_NAME(state, { recipe, value }) {
    recipe.name = value;
  },
  SET_RECIPE_INGREDIENTS(state, { recipe, value }) {
    recipe.ingredients = value;
  },
  SET_RECIPE_DIRECTIONS(state, { recipe, value }) {
    recipe.directions = value;
  },
  SET_RECIPE_PREP_TIME_MINUTES(state, { recipe, value }) {
    recipe.prepTimeMinutes = value;
  },
  SET_RECIPE_COOK_TIME_MINUTES(state, { recipe, value }) {
    recipe.cookTimeMinutes = value;
  },
  ADD_CATEGORY_TO_RECIPE(state, { recipe, cleanedCategoryName }) {
    recipe.categories.push(cleanedCategoryName);
  },
  REMOVE_CATEGORY_FROM_RECIPE(state, { recipe, categoryIndex }) {
    recipe.categories.splice(categoryIndex, 1);
  },
  SET_RECENT_RECIPES(state, recentRecipes) {
    state.recentRecipes = recentRecipes;
  },
  SET_RECIPES_LIST(state, recipes) {
    state.recipesList = recipes;
  },
  SET_RECIPES_LIST_TOTAL_COUNT(state, totalCount) {
    state.recipesListTotalCount = totalCount;
  },
  SET_RECIPES_SEARCH_PARAMETERS_NAME_SEARCH(state, nameSearch) {
    state.recipesSearchParameters.nameSearch = nameSearch;
  },
  SET_RECIPES_SEARCH_PARAMETERS_CATEGORY_SEARCH(state, categorySearch) {
    state.recipesSearchParameters.categorySearch = categorySearch;
  },
  SET_RECIPES_SEARCH_PARAMETERS_PAGE(state, page) {
    state.recipesSearchParameters.page = page;
  },
  SET_RECIPES_SEARCH_PARAMETERS_TAKE(state, take) {
    state.recipesSearchParameters.take = take;
  },
  SET_RECIPES_SEARCH_PARAMETERS_SORT(state, nameSort) {
    state.recipesSearchParameters.nameSort = nameSort;
  },
};
/* eslint-enable no-param-reassign */
