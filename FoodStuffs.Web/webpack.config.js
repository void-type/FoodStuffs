const path = require("path");
const webpack = require("webpack");

module.exports = {
    context: path.resolve(__dirname, "ClientApp"),
    entry: {
        app: "./app.js"
    },
    output: {
        path: path.resolve(__dirname, "wwwroot/dist"),
        filename: "[name].bundle.js"
    }
};

// Setup webpack to compile js and scss into wwwroot/dist