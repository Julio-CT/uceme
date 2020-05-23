"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_router_1 = require("react-router");
const Layout_1 = require("./components/Layout");
const Home_1 = require("./components/Home");
const FetchData_1 = require("./components/FetchData");
const Counter_1 = require("./components/Counter");
const AuthorizeRoute_1 = require("./components/api-authorization/AuthorizeRoute");
const ApiAuthorizationRoutes_1 = require("./components/api-authorization/ApiAuthorizationRoutes");
const ApiAuthorizationConstants_1 = require("./components/api-authorization/ApiAuthorizationConstants");
require("./App.css");
require("./custom.css");
function App() {
    return (React.createElement(Layout_1.Layout, null,
        React.createElement(react_router_1.Route, { exact: true, path: '/', component: Home_1.Home }),
        React.createElement(react_router_1.Route, { path: '/counter', component: Counter_1.Counter }),
        React.createElement(AuthorizeRoute_1.default, { path: '/fetch-data', component: FetchData_1.FetchData }),
        React.createElement(react_router_1.Route, { path: ApiAuthorizationConstants_1.ApplicationPaths.ApiAuthorizationPrefix, component: ApiAuthorizationRoutes_1.default })));
}
exports.default = App;
//# sourceMappingURL=App.js.map