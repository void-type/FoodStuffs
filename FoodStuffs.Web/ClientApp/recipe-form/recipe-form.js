var appState = require("../site/state.js");

const component = new Vue({
    el: "#recipe-form",
    data: appState,
    methods: {
        cancel: function () {
            appState.currentRecipe = null;
        }
    }
});

module.exports = component;