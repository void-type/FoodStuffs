import Recipe from "../models/recipe";
import RecipesSearchParameters from "../models/recipesSearchParameters";
import { applicationName } from "../models/clientSettings"

export default {
  applicationName: applicationName,
  currentRecipe: new Recipe(),
  messages: [],
  fieldsInError: [],
  isError: false,
  recipesList: [],
  recipesListPage: 0,
  recipesListTake: 0,
  recipesListTotalCount: 0,
  recentRecipes: [],
  recipesSearchParameters: new RecipesSearchParameters()
}