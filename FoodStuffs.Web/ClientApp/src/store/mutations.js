export default {

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
        state.isError = status;
    },

    addCategoryToCurrentRecipe(state, newCategoryName) {
        state.currentRecipe.categories.push(newCategoryName);
    },

    removeCategoryFromCurrentRecipe(state, indexOfCategoryToRemove) {
        state.currentRecipe.categories.splice(indexOfCategoryToRemove, 1);
    },

    addCurrentRecipeToRecents(state) {
        if (state.recipes.indexOf(state.currentRecipe) < 0) {
            return;
        }

        const recentRecipeIndex = state.recentRecipes
            .indexOf(state.recentRecipes
                .filter(listItem => listItem.id === state.currentRecipe.id)[0]);

        if (recentRecipeIndex > -1) {
            state.recentRecipes.splice(recentRecipeIndex, 1);
        }

        if (state.currentRecipe.id > 0) {
            const recentRecipeListItem = Object.assign({}, state.currentRecipe);

            state.recentRecipes.unshift(recentRecipeListItem);
        }

        if (state.recentRecipes.length > 3) {
            state.recentRecipes.pop();
        }
    }
}