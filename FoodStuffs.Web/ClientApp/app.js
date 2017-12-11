require("./components/site/site.scss");

require("./components/message-center/message-center.js");
require("./components/recipe-form/recipe-form.js");
require("./components/recipe-table/recipe-table.js");

new Vue({
    el: "#app-main",
    template: require("./components/site/appMain.html")
});