import Recipe from "../models/recipe";
import RecipesSearchParameters from "../models/recipesSearchParameters";

export default {
  applicationName: "",
  currentRecipe: new Recipe(),
  messages: [],
  fieldsInError: [],
  isError: false,
  recipesList: [],
  recipesListPage: 0,
  recipesListTake: 0,
  recipesListTotalCount: 0,
  recentRecipeIds: [],
  recipesSearchParameters: new RecipesSearchParameters()
}