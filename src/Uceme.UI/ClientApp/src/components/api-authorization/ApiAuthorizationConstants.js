"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ApplicationName = 'Uceme.UI';
exports.QueryParameterNames = {
    ReturnUrl: 'returnUrl',
    Message: 'message'
};
exports.LogoutActions = {
    LogoutCallback: 'logout-callback',
    Logout: 'logout',
    LoggedOut: 'logged-out'
};
exports.LoginActions = {
    Login: 'login',
    LoginCallback: 'login-callback',
    LoginFailed: 'login-failed',
    Profile: 'profile',
    Register: 'register'
};
const prefix = '/authentication';
exports.ApplicationPaths = {
    DefaultLoginRedirectPath: '/',
    ApiAuthorizationClientConfigurationUrl: `/_configuration/${exports.ApplicationName}`,
    ApiAuthorizationPrefix: prefix,
    Login: `${prefix}/${exports.LoginActions.Login}`,
    LoginFailed: `${prefix}/${exports.LoginActions.LoginFailed}`,
    LoginCallback: `${prefix}/${exports.LoginActions.LoginCallback}`,
    Register: `${prefix}/${exports.LoginActions.Register}`,
    Profile: `${prefix}/${exports.LoginActions.Profile}`,
    LogOut: `${prefix}/${exports.LogoutActions.Logout}`,
    LoggedOut: `${prefix}/${exports.LogoutActions.LoggedOut}`,
    LogOutCallback: `${prefix}/${exports.LogoutActions.LogoutCallback}`,
    IdentityRegisterPath: '/Identity/Account/Register',
    IdentityManagePath: '/Identity/Account/Manage'
};
//# sourceMappingURL=ApiAuthorizationConstants.js.map