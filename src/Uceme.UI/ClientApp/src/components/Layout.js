"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const reactstrap_1 = require("reactstrap");
const NavMenu_1 = require("./NavMenu");
class Layout extends React.Component {
    render() {
        return (React.createElement("div", null,
            React.createElement(NavMenu_1.NavMenu, null),
            React.createElement(reactstrap_1.Container, null, this.props.children)));
    }
}
exports.Layout = Layout;
Layout.displayName = Layout.name;
//# sourceMappingURL=Layout.js.map