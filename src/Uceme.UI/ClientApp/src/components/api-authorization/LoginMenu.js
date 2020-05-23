"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_1 = require("react");
const reactstrap_1 = require("reactstrap");
const react_router_dom_1 = require("react-router-dom");
const AuthorizeService_1 = require("./AuthorizeService");
const ApiAuthorizationConstants_1 = require("./ApiAuthorizationConstants");
class LoginMenu extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isAuthenticated: false,
            userName: null
        };
        this._subscription = 0;
    }
    componentDidMount() {
        this._subscription = AuthorizeService_1.default.subscribe(() => this.populateState());
        this.populateState();
    }
    componentWillUnmount() {
        AuthorizeService_1.default.unsubscribe(this._subscription);
    }
    populateState() {
        return __awaiter(this, void 0, void 0, function* () {
            const [isAuthenticated, user] = yield Promise.all([AuthorizeService_1.default.isAuthenticated(), AuthorizeService_1.default.getUser()]);
            this.setState({
                isAuthenticated,
                userName: user && user.name
            });
        });
    }
    render() {
        const { isAuthenticated, userName } = this.state;
        if (!isAuthenticated) {
            const registerPath = `${ApiAuthorizationConstants_1.ApplicationPaths.Register}`;
            const loginPath = `${ApiAuthorizationConstants_1.ApplicationPaths.Login}`;
            return this.anonymousView(registerPath, loginPath);
        }
        else {
            const profilePath = `${ApiAuthorizationConstants_1.ApplicationPaths.Profile}`;
            const logoutPath = { pathname: `${ApiAuthorizationConstants_1.ApplicationPaths.LogOut}`, state: { local: true } };
            return this.authenticatedView(userName, profilePath, logoutPath);
        }
    }
    authenticatedView(userName, profilePath, logoutPath) {
        return (React.createElement(react_1.Fragment, null,
            React.createElement(reactstrap_1.NavItem, null,
                React.createElement(reactstrap_1.NavLink, { tag: react_router_dom_1.Link, className: "text-dark", to: profilePath },
                    "Hello ",
                    userName)),
            React.createElement(reactstrap_1.NavItem, null,
                React.createElement(reactstrap_1.NavLink, { tag: react_router_dom_1.Link, className: "text-dark", to: logoutPath }, "Logout"))));
    }
    anonymousView(registerPath, loginPath) {
        return (React.createElement(react_1.Fragment, null,
            React.createElement(reactstrap_1.NavItem, null,
                React.createElement(reactstrap_1.NavLink, { tag: react_router_dom_1.Link, className: "text-dark", to: registerPath }, "Register")),
            React.createElement(reactstrap_1.NavItem, null,
                React.createElement(reactstrap_1.NavLink, { tag: react_router_dom_1.Link, className: "text-dark", to: loginPath }, "Login"))));
    }
}
exports.LoginMenu = LoginMenu;
//# sourceMappingURL=LoginMenu.js.map