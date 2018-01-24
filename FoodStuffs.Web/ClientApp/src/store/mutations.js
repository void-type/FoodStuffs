export default {
    clearErrors(state) {
        state.isError = false;
        state.fieldsInError = [];
        state.messages = [];
    },

    selectRecipe(state, recipe) {
        state.currentRecipe = recipe;
    },

    setRecipesList(state, recipes) {
        state.recipes = recipes;
    },

    setMessage(state, message) {
        state.messages = [message];
    },

    setMessages(state, messages) {
        state.messages = messages;
    },

    setFieldsInError(state, fieldNames) {
        state.fieldsInError = fieldNames;
    },

    setIsError(state, status) {
        state.isError = status || true;
    },

    addCategoryToCurrentRecipe(state, newCategoryName) {
        state.currentRecipe.categories.push(newCategoryName);
    },

    removeCategoryFromCurrentRecipe(state, indexOfCategoryToRemove) {
        state.currentRecipe.categories.splice(indexOfCategoryToRemove, 1);
    },

    addCurrentRecipeToRecents(state) {
        const recipeIsRecent = state.recentRecipes.indexOf(state.currentRecipe);

        if (recipeIsRecent > -1) {
            state.recentRecipes.splice(recipeIsRecent, 1);
        }

        if (state.currentRecipe.id > 0) {
            state.recentRecipes.unshift(state.currentRecipe);
        }

        if (state.recentRecipes.length > 3) {
            state.recentRecipes.pop();
        }
    }
}