"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_1 = require("react");
class Home extends react_1.Component {
    render() {
        return (React.createElement("div", { className: "App" },
            React.createElement("header", { className: "App-header" },
                React.createElement("img", { src: "./logo.svg", className: "App-logo", alt: "logo" }),
                React.createElement("p", null,
                    "Edit ",
                    React.createElement("code", null, "src/App.tsx"),
                    " and save to reload."),
                React.createElement("a", { className: "App-link", href: "https://reactjs.org", target: "_blank", rel: "noopener noreferrer" }, "Learn React"))));
    }
}
exports.Home = Home;
Home.displayName = Home.name;
//# sourceMappingURL=Home.js.map