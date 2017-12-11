/// <binding ProjectOpened='Watch - Development' />

const path = require("path");
const webpack = require("webpack");
const CopyWebpackPlugin = require("copy-webpack-plugin");
const ExtractTextPlugin = require("extract-text-webpack-plugin");

module.exports = {
    context: path.resolve(__dirname),
    entry: {
        "app": "./ClientApp/app.js",
        "app.min": "./ClientApp/app.js"
    },
    devtool: "source-map",
    output: {
        path: path.resolve(__dirname, "wwwroot/dist"),
        filename: "[name].js"
    },
    module: {
        rules: [
            {
                test: /\.scss$/,
                use: ExtractTextPlugin.extract({
                    fallback: "style-loader",
                    use: [
                        {
                            loader: "css-loader",
                            options: { minimize: true }
                        }, {
                            loader: "sass-loader"
                        }
                    ]
                })
            },
            {
                test: /\.html$/,
                use: {
                    loader: "html-loader",
                    options: {
                        attrs: [":data-src"]
                    }
                }
            },
            {
                test: /\.(jpg|png)$/,
                use: {
                    loader: "file-loader",
                    //loader: 'file-loader?name=images/[name].[ext]'
                }
            },

        ]
    },
    plugins: [
        // Put styles in a separate file
        new ExtractTextPlugin("style.min.css"),

        // Minimize js
        new webpack.optimize.UglifyJsPlugin({
            include: /\.min\.js$/,
            minimize: true
        }),

        // Copy npm vendor assets to wwwroot
        new CopyWebpackPlugin(
            [
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