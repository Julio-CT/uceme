const React = require('react');

function Helmet(props) {
  // Render children and ignore side-effects in tests
  return React.createElement(React.Fragment, null, props.children);
}

module.exports = {
  Helmet,
};
