import Recipe from '../models/recipe';
import RecipesSearchParameters from '../models/recipesSearchParameters';

export default {
  applicationName: '',
  currentRecipe: new Recipe(),
  fieldsInError: [],
  messages: [],
  messageIsError: false,
  recentRecipeIds: [],
  recipesList: [],
  recipesListTotalCount: 0,
  recipesSearchParameters: new RecipesSearchParameters(),
};
