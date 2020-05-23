"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_1 = require("react");
const react_router_1 = require("react-router");
const Login_1 = require("./Login");
const Logout_1 = require("./Logout");
const ApiAuthorizationConstants_1 = require("./ApiAuthorizationConstants");
class ApiAuthorizationRoutes extends react_1.Component {
    render() {
        return (React.createElement(react_1.Fragment, null,
            React.createElement(react_router_1.Route, { path: ApiAuthorizationConstants_1.ApplicationPaths.Login, render: () => loginAction(ApiAuthorizationConstants_1.LoginActions.Login) }),
            React.createElement(react_router_1.Route, { path: ApiAuthorizationConstants_1.ApplicationPaths.LoginFailed, render: () => loginAction(ApiAuthorizationConstants_1.LoginActions.LoginFailed) }),
            React.createElement(react_router_1.Route, { path: ApiAuthorizationConstants_1.ApplicationPaths.LoginCallback, render: () => loginAction(ApiAuthorizationConstants_1.LoginActions.LoginCallback) }),
            React.createElement(react_router_1.Route, { path: ApiAuthorizationConstants_1.ApplicationPaths.Profile, render: () => loginAction(ApiAuthorizationConstants_1.LoginActions.Profile) }),
            React.createElement(react_router_1.Route, { path: ApiAuthorizationConstants_1.ApplicationPaths.Register, render: () => loginAction(ApiAuthorizationConstants_1.LoginActions.Register) }),
            React.createElement(react_router_1.Route, { path: ApiAuthorizationConstants_1.ApplicationPaths.LogOut, render: () => logoutAction(ApiAuthorizationConstants_1.LogoutActions.Logout) }),
            React.createElement(react_router_1.Route, { path: ApiAuthorizationConstants_1.ApplicationPaths.LogOutCallback, render: () => logoutAction(ApiAuthorizationConstants_1.LogoutActions.LogoutCallback) }),
            React.createElement(react_router_1.Route, { path: ApiAuthorizationConstants_1.ApplicationPaths.LoggedOut, render: () => logoutAction(ApiAuthorizationConstants_1.LogoutActions.LoggedOut) })));
    }
}
exports.default = ApiAuthorizationRoutes;
function loginAction(name) {
    return (React.createElement(Login_1.Login, { action: name }));
}
function logoutAction(name) {
    return (React.createElement(Logout_1.Logout, { action: name }));
}
//# sourceMappingURL=ApiAuthorizationRoutes.js.map