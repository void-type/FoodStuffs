require("./components/site/site.scss");

require("./components/message-center/message-center.js");
require("./components/recipe-form/recipe-form.js");
require("./components/recipe-table/recipe-table.js");

var appStateData = require("./store/appState.js");

var appRoot = new Vue({
    el: "#app-main",
    template: require("./components/site/appMain.html"),
    data: appStateData
});