import webApi from "./webApi"
import Recipe from "../models/recipe"

const defaultCallbacks = {
    onSuccess(context, data) {
        context.dispatch("fetchRecipes");
        context.commit("selectRecipe", new Recipe());
        context.commit("setMessage", data.message);
    },

    onFailure(context, response) {
        context.commit("setIsError");

        if (response.status >= 500) {
            context.commit("setMessage", response.data.message);
        } else {
            context.commit("setFieldsInError", response.data.items.map((item) => item.fieldName));
            context.commit("setMessages", response.data.items.map((item) => item.errorMessage));
        }
    }
}

export default {
    fetchRecipes(context) {
        webApi.listRecipes(
            data => context.commit("setRecipesList", data.items),
            response => defaultCallbacks.onFailure(context, response));
    },

    saveRecipe(context, recipe) {
        context.commit("clearErrors");

        if (recipe.id === undefined || recipe.id < 1) {
            webApi.createRecipe(
                recipe,
                data => defaultCallbacks.onSuccess(context, data),
                response => defaultCallbacks.onFailure(context, response));
        } else {
            webApi.updateRecipe(
                recipe,
                data => defaultCallbacks.onSuccess(context, data),
                response => defaultCallbacks.onFailure(context, response));
        }
    },

    deleteRecipe(context, recipe) {
        context.commit("clearErrors");
        if (confirm("Are you sure you want to delete this recipe?")) {
            webApi.deleteRecipe(
                recipe,
                data => defaultCallbacks.onSuccess(context, data),
                response => defaultCallbacks.onFailure(context, response));
        }
    },

    selectRecipe(context, recipe) {
        context.commit("clearErrors");
        context.commit("addCurrentRecipeToRecents");
        context.commit("selectRecipe", recipe || new Recipe());
    },

    addCategoryToCurrentRecipe(context, newCategoryName) {

        newCategoryName = newCategoryName
            .trim()
            .split(" ")
            .filter(word => word.length > 0)
            .map(word => word[0].toUpperCase() + word.substring(1))
            .join(" ");

        const categoryDoesNotExist = context.state.currentRecipe.categories
            .map((value) => value.toUpperCase())
            .indexOf(newCategoryName.toUpperCase()) < 0;

        if (categoryDoesNotExist && newCategoryName.length > 0) {
            context.commit("addCategoryToCurrentRecipe", newCategoryName);
        }
    },

    removeCategoryFromCurrentRecipe(context, categoryToRemove) {
        const index = context.state.currentRecipe.categories.indexOf(categoryToRemove);

        if (index > -1) {
            context.commit("removeCategoryFromCurrentRecipe", index);
        }
    }
}