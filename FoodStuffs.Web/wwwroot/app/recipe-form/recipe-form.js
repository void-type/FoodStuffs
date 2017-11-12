let recipeEditor = new Vue({
    el: "#recipe-form",
    data: appState,
    methods: {
        cancel: function () {
            appState.currentRecipe = null;
        }
    }
});