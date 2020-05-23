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
var __rest = (this && this.__rest) || function (s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
            if (e.indexOf(p[i]) < 0 && Object.prototype.propertyIsEnumerable.call(s, p[i]))
                t[p[i]] = s[p[i]];
        }
    return t;
};
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_router_dom_1 = require("react-router-dom");
const ApiAuthorizationConstants_1 = require("./ApiAuthorizationConstants");
const AuthorizeService_1 = require("./AuthorizeService");
class AuthorizeRoute extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            ready: false,
            authenticated: false
        };
    }
    componentDidMount() {
        this._subscription = AuthorizeService_1.default.subscribe(() => this.authenticationChanged());
        this.populateAuthenticationState();
    }
    componentWillUnmount() {
        AuthorizeService_1.default.unsubscribe(this._subscription);
    }
    render() {
        const { ready, authenticated } = this.state;
        var link = document.createElement("a");
        link.href = this.props.path;
        const returnUrl = `${link.protocol}//${link.host}${link.pathname}${link.search}${link.hash}`;
        const redirectUrl = `${ApiAuthorizationConstants_1.ApplicationPaths.Login}?${ApiAuthorizationConstants_1.QueryParameterNames.ReturnUrl}=${encodeURI(returnUrl)}`;
        if (!ready) {
            return React.createElement("div", null);
        }
        else {
            const _a = this.props, { component: Component } = _a, rest = __rest(_a, ["component"]);
            return React.createElement(react_router_dom_1.Route, Object.assign({}, rest, { render: (props) => {
                    if (authenticated) {
                        return React.createElement(React.Component, Object.assign({}, props));
                    }
                    else {
                        return React.createElement(react_router_dom_1.Redirect, { to: redirectUrl });
                    }
                } }));
        }
    }
    populateAuthenticationState() {
        return __awaiter(this, void 0, void 0, function* () {
            const authenticated = yield AuthorizeService_1.default.isAuthenticated();
            this.setState({ ready: true, authenticated });
        });
    }
    authenticationChanged() {
        return __awaiter(this, void 0, void 0, function* () {
            this.setState({ ready: false, authenticated: false });
            yield this.populateAuthenticationState();
        });
    }
}
exports.default = AuthorizeRoute;
//# sourceMappingURL=AuthorizeRoute.js.map