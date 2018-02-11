import Recipe from "../models/recipe";

export default {
  messages: [],
  recipes: [],
  recipesTotalCount: 0,
  recipesPage: 0,
  recipesTake: 0,
  fieldsInError: [],
  currentRecipe: new Recipe(),
  isError: false,
  recentRecipes: []
}