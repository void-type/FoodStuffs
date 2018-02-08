import Recipe from "../models/recipe";

export default {
  messages: [],
  recipes: [],
  fieldsInError: [],
  currentRecipe: new Recipe(),
  isError: false,
  recentRecipes: []
}