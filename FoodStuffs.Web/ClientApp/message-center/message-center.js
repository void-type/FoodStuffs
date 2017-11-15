var appState = require("../site/state.js");

const component = new Vue({
    el: "#message-center",
    data: appState
});

module.exports = component;