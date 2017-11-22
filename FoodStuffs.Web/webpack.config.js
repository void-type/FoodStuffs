/// <binding ProjectOpened='Watch - Development' />
var path = require("path");
var webpack = require("webpack");

module.exports = {
    context: path.resolve(__dirname, "ClientApp/"),
    entry: {
        "app": "./app.js",
        "app.min": "./app.js"
    },
    devtool: "source-map",
    output: {
        path: path.resolve(__dirname, "wwwroot/dist"),
        filename: "[name].js"
    },
    plugins: [
        new webpack.optimize.UglifyJsPlugin({
            include: /\.min\.js$/,
            minimize: true
        })
    ]
};