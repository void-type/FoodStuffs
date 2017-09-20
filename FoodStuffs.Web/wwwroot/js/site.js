let appState = {
    recipes: null,
    currentRecipe: null,

    refresh: function () {
        axios.get("api/recipes/list")
            .then(function (response) {
                appState.recipes = response.data.items;
            });
    }
};

appState.refresh();

let recipeTable = new Vue({
    el: "#recipe-table",
    data: appState,
    methods: {
        selectRecipe: function (recipe) {
            appState.currentRecipe = recipe;
        }
    }
});

let recipeEditor = new Vue({
    el: "#recipe-form",
    data: appState,
    methods: {
        clearCurrent: function() {
            appState.currentRecipe = null;
        }
    }
});