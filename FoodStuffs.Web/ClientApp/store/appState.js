var appState = {
    recipes: new Array(),
    messages: new Array(),
    fieldsInError: new Array(),
    currentRecipe: {},
    isError: false,

    list: function () {
        appState.messages = new Array();

        axios.get("api/recipes/list")
            .then(function (response) {
                appState.recipes = response.data.items;
            })
            .catch(function () {
                appState.isError = true;
                appState.messages = error.response.data.items;
            });
    },

    createRecipe: function (recipe) {
        appState.messages = new Array();

        axios.put("api/recipes", recipe)
            .then(appState.success)
            .catch(appState.failure);
    },

    updateRecipe: function (recipe) {
        appState.messages = new Array();

        axios.post("api/recipes", recipe)
            .then(appState.success)
            .catch(appState.failure);
    },

    deleteRecipe: function (recipe) {
        appState.messages = new Array();

        axios.delete("api/recipes", recipe.id)
            .then(appState.success)
            .catch(appState.failure);
    },

    success: function (response) {
        appState.isError = false;
        appState.messages = [response.data.message];
        appState.list();
    },

    failure: function (error) {
        appState.isError = true;
        appState.messages = error.response.data.items;
        appState.list();
    },

    recipeSelected: function(recipe) {
        appState.currentRecipe = recipe;
    }
};

appState.list();

module.exports = appState;