const path = require('path');

module.exports = {
  outputDir: '../FoodStuffs.Web/wwwroot',
  configureWebpack: {
    devtool: 'cheap-module-source-map',
  },
  chainWebpack: (config) => {
    config
      .plugin('html')
      .tap(() => [{
        filename: 'app.html',
        template: path.resolve('src/app.html'),
        hash: true,
        minify: {
          removeComments: true,
          collapseWhitespace: true,
          removeAttributeQuotes: true,
        },
        chunksSortMode: 'dependency',
      }]);
  },
};
