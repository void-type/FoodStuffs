let recipeTable = new Vue({
    el: "#recipe-table",
    data: appState,
    methods: {
        select: function (recipe) {
            appState.currentRecipe = recipe;
        },
        newRecipe: function () {
            appState.currentRecipe = {};
        }
    }
});