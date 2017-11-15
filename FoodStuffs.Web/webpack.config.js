/// <binding BeforeBuild='Watch - Development' />
var path = require("path");

module.exports = {
    context: path.resolve(__dirname, "ClientApp/"),
    entry: "./app.js",
    output: {
        filename: "app.js",
        path: path.resolve(__dirname, "wwwroot/dist")
    }
};