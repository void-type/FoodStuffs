const HtmlWebpackPlugin = require('html-webpack-plugin');
const path = require('path');

module.exports = {
  configureWebpack: {
    output: {
      filename: 'app.js',
      path: path.resolve(__dirname, '../wwwroot'),
    },
    plugins: [
      // generate dist index.html with correct asset hash for caching.
      // you can customize output by editing /index.html
      // see https://github.com/ampedandwired/html-webpack-plugin
      new HtmlWebpackPlugin({
        filename: 'app.html',
        template: 'public/index.html',
        inject: true,
        minify: {
          removeComments: true,
          collapseWhitespace: true,
          removeAttributeQuotes: true,
          // more options:
          // https://github.com/kangax/html-minifier#options-quick-reference
        },
        // necessary to consistently work with multiple chunks via CommonsChunkPlugin
        chunksSortMode: 'dependency',
      }),
    ],
  },
};
