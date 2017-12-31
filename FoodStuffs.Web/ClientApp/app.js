require("./components/site/site.scss");

require("./components/message-center/message-center.js");
require("./components/recipe-form/recipe-form.js");
require("./components/recipe-table/recipe-table.js");

var appState = require("./store/appState.js");

var Recipe = require("./store/createdRecipeModel.js");

var appRoot = new Vue({
    el: "#app-main",
    template: require("./components/site/appMain.html"),
    created: function () {
        this.list();
    },
    data: appState,
    methods: {
        list: function () {
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
                .then(this.success)
                .catch(this.failure);
        },

        updateRecipe: function (recipe) {
            appState.messages = new Array();

            axios.post("api/recipes", recipe)
                .then(this.success)
                .catch(this.failure);
        },

        deleteRecipe: function (recipe) {
            appState.messages = new Array();

            axios.delete("api/recipes", recipe.id)
                .then(this.success)
                .catch(this.failure);
        },

        success: function (response) {
            appState.isError = false;

            appState.messages = [response.data.message];

            appState.fieldsInError = new Array();

            this.list();
        },

        failure: function (error) {
            appState.isError = true;

            appState.messages = error.response.data.items.map(function (item) {
                return item.errorMessage;
            });

            appState.fieldsInError = error.response.data.items.map(function (item) {
                return item.fieldName;
            });

            this.list();
        },

        recipeSelected: function (recipe) {
            this.clearErrors();
            appRoot.currentRecipe = recipe;
        },

        clearErrors: function () {
            appState.fieldsInError = new Array();
            appState.messages = new Array();
        }
    }
});