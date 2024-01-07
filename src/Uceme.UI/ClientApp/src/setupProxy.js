const morgan = require('morgan');
const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function (app) {
  app.use(
    '/api',
    createProxyMiddleware({
      target: 'http://localhost:5001', // whichever the port of the API is
      changeOrigin: true,
    })
  );

  app.use(morgan('combined'));
};
