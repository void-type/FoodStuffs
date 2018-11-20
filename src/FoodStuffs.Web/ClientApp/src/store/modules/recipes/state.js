import recipeApiModels from '../../../models/RecipeApiModels';

export default {
  currentRecipe: new recipeApiModels.GetResponse(),
  recentRecipes: [],
  recipesList: [],
  recipesListTotalCount: 0,
  recipesSearchParameters: recipeApiModels.GetRequestSortTypes,
};
