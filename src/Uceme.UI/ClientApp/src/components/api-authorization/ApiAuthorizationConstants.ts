export const ApplicationName = 'Uceme.UI';

export const QueryParameterNames = {
  ReturnUrl: 'returnUrl',
  Message: 'message',
};

export const LogoutActions = {
  LogoutCallback: 'logout-callback',
  Logout: 'logout',
  LoggedOut: 'logged-out',
};

export const LoginActions = {
  Login: 'login',
  LoginCallback: 'login-callback',
  LoginFailed: 'login-failed',
  Profile: 'profile',
  Register: 'register',
};

export type ResultState = {
  returnUrl: string;
};

export type ResultModel = {
  message: string | undefined | null;
  state: ResultState | null;
  status: string;
};

export type Arguments = {
  useReplaceToNavigate: boolean;
  data: ResultState | null;
};

export type Callback = {
  callback: Function;
  subscription: number;
};

const prefix = '/authentication';

const ApplicationPaths = {
  DefaultLoginRedirectPath: '/',
  ApiAuthorizationClientConfigurationUrl: `_configuration/${ApplicationName}`,
  ApiAuthorizationPrefix: prefix,
  Login: `${prefix}/${LoginActions.Login}`,
  LoginFailed: `${prefix}/${LoginActions.LoginFailed}`,
  LoginCallback: `${prefix}/${LoginActions.LoginCallback}`,
  Register: `${prefix}/${LoginActions.Register}`,
  Profile: `${prefix}/${LoginActions.Profile}`,
  LogOut: `${prefix}/${LogoutActions.Logout}`,
  LoggedOut: `${prefix}/${LogoutActions.LoggedOut}`,
  LogOutCallback: `${prefix}/${LogoutActions.LogoutCallback}`,
  IdentityRegisterPath: '/Identity/Account/Register',
  IdentityManagePath: '/Identity/Account/Manage',
};

export default ApplicationPaths;
