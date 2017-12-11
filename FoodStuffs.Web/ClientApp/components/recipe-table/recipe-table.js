var appState = require("../../store/appState.js");
require("./recipe-table.scss");

Vue.component("recipe-table", {
    template: require("./recipe-table.html"),
    data: function () {
        return appState;
    },
    methods: {
        selectRecipe: function (recipe) {
            appState.currentRecipe = recipe;
        },

        newRecipe: function () {
            appState.currentRecipe = {};
        }
    }
});