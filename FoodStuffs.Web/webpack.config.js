/// <binding ProjectOpened='Watch - Development' />
const path = require("path");
const webpack = require("webpack");
const CopyWebpackPlugin = require("copy-webpack-plugin");

module.exports = {
    context: path.resolve(__dirname),
    entry: {
        "app": "./ClientApp/app.js",
        "app.min": "./ClientApp/app.js"
    },
    devtool: "source-map",
    output: {
        path: path.resolve(__dirname, "wwwroot"),
        filename: "dist/[name].js"
    },
    plugins: [
        new webpack.optimize.UglifyJsPlugin({
            include: /\.min\.js$/,
            minimize: true
        }),

        // Copy npm vendor assets to wwwroot
        new CopyWebpackPlugin([

            // {output}/to/file.txt
            { from: "node_modules/axios/dist/*", to: "vendor/axios", flatten: true },
            { from: "node_modules/vue/dist/*", to: "vendor/vue", flatten: true },
            { from: "node_modules/vue-router/dist/*", to: "vendor/vue-router", flatten: true }
        ],
            {
                ignore: [
                    // Doesn't copy any files with a txt extension
                    "*.txt",

                    // Doesn't copy any files with a md extension
                    "*.md"
                ]
            })
    ]
};