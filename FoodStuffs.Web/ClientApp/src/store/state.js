import Recipe from "../models/recipe.js"

export default {
    messages: [],
    recipes: [],
    fieldsInError: [],
    currentRecipe: new Recipe(),
    isError: false,
    recentRecipes: []
}