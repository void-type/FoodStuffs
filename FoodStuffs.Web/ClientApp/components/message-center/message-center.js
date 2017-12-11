var appState = require("../../store/appState.js");
require("./message-center.scss");

Vue.component("message-center", {
    template: require("./message-center.html"),
    data: function () {
        return appState;
    }
});