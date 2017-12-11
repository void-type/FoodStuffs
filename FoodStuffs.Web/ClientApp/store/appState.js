var appState = {
    recipes: new Array(),
    currentRecipe: {},
    messages: new Array(),
    isError: false,

    clearMessages: function () {
        appState.messages = new Array();
    },

    success: function (message) {
        appState.isError = false;
        appState.messages = appState.messages.push(message);
    },

    error: function (messages) {
        appState.isError = true;
        appState.messages = messages;
    },

    list: function () {
        appState.clearMessages();

        axios.get("api/recipes/list")
            .then(function (response) {
                appState.recipes = response.data.items;
            })
            .catch(function (error) {
                appState.error(error.response.data.items);
            });;
    },

    create: function (recipe) {
        appState.clearMessages();

        axios.put("api/recipes", recipe)
            .then(function (response) {
                appState.success(response.data.message);
                appState.list();
            })
            .catch(function (error) {
                appState.error(error.response.data.items);
            });
    },

    update: function (recipe) {
        appState.clearMessages();

        axios.post("api/recipes", recipe)
            .then(function (response) {
                appState.success(response.data.message);
                appState.list();
            })
            .catch(function (error) {
                appState.error(error.response.data.items);
            });
    },

    delete: function (recipeId) {
        appState.clearMessages();

        axios.delete("api/recipes", recipeId)
            .then(function (response) {
                appState.success(response.data.message);
                appState.list();
            })
            .catch(function (error) {
                appState.error(error.response.data.items);
            });
    }
};

appState.list();

module.exports = appState;