import Recipe from '../models/recipe';
import RecipesSearchParameters from '../models/recipesSearchParameters';

export default {
  currentRecipe: new Recipe(),
  recentRecipes: [],
  recipesList: [],
  recipesListTotalCount: 0,
  recipesSearchParameters: new RecipesSearchParameters(),
};
