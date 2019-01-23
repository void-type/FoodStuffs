const path = require('path');

module.exports = {
  outputDir: '../wwwroot',
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
