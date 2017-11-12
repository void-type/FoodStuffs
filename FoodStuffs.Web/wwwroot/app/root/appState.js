let appState = {
    recipes: null,
    currentRecipe: null,
    messages: null,
    isError: false,

    clearMessages: function () {
        appState.messages = null;
    },

    success: function (messages) {
        appState.isError = false;
        appState.messages = messages;
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
            });
    },

    create: function (recipe) {
        appState.clearMessages();

        axios.put("api/recipes", recipe)
            .then(function (response) {
                appState.success(response.data.items);
                appState.list();
            })
            .catch(function (response) {
                appState.error(response.data.items);
            });
    },

    update: function (recipe) {
        appState.clearMessages();

        axios.post("api/recipes", recipe)
            .then(function (response) {
                appState.success(response.data.items);
                appState.list();
            })
            .catch(function (response) {
                appState.error(response.data.items);
            });
    },

    delete: function (recipeId) {
        appState.clearMessages();

        axios.delete("api/recipes", recipeId)
            .then(function (response) {
                appState.success(response.data.items);
                appState.list();
            })
            .catch(function (response) {
                appState.error(response.data.items);
            });
    }
};

appState.list();