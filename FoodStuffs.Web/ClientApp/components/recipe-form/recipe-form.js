var appState = require("../../store/appState.js");
require("./recipe-form.scss");

Vue.component("recipe-form", {
    template: require("./recipe-form.html"),
    data: function () {
        return appState;
    },
    methods: {
        save: function (recipe) {
            if (appState.currentRecipe.id === undefined || appState.currentRecipe.id < 1) {
                appState.create(recipe);
            } else {
                appState.update(recipe);
            }
        },

        cancel: function () {
            appState.currentRecipe = null;
        }
    }
});