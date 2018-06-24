import Recipe from '../models/recipe';
import RecipesSearchParameters from '../models/recipesSearchParameters';

export default {
  applicationName: '',
  userName: '',
  currentRecipe: new Recipe(),
  fieldsInError: [],
  messages: [],
  messageIsError: false,
  recentRecipes: [],
  recipesList: [],
  recipesListTotalCount: 0,
  recipesSearchParameters: new RecipesSearchParameters(),
};
