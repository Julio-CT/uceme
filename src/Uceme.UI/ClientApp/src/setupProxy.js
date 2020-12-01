const morgan = require('morgan');
const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function (app) {
  app.use(
    '/api',
    createProxyMiddleware({
      target: 'http://localhost:4155',
      changeOrigin: true,
    })
  );

  app.use(morgan('combined'));
};
