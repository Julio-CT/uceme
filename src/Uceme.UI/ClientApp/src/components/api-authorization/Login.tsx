import * as React from 'react';
import { ReactElement } from 'react';
import authService, { AuthenticationResultStatus } from './AuthorizeService';

import ApplicationPaths, {
  LoginActions,
  QueryParameterNames,
  ResultModel,
  ResultState,
} from './ApiAuthorizationConstants';

// The main responsibility of this component is to handle the user's login process.
// This is the starting point for the login process. Any component that needs to authenticate
// a user can simply perform a redirect to this component with a returnUrl query parameter and
// let the component perform the login and return back to the return url.
type LoginState = {
  message: string | null | undefined;
};

type LoginProps = {
  action: string;
};

class Login extends React.Component<LoginProps, LoginState> {
  static redirectToRegister(): void {
    Login.redirectToApiAuthorizationPath(
      `${ApplicationPaths.IdentityRegisterPath}?${
        QueryParameterNames.ReturnUrl
      }=${encodeURI(ApplicationPaths.Login)}`
    );
  }

  static redirectToProfile(): void {
    Login.redirectToApiAuthorizationPath(ApplicationPaths.IdentityManagePath);
  }

  static redirectToApiAuthorizationPath(apiAuthorizationPath: string): void {
    const redirectUrl = `${window.location.origin}${apiAuthorizationPath}`;
    // It's important that we do a replace here so that when the user hits the back arrow on the
    // browser he gets sent back to where it was on the app instead of to an endpoint on this
    // component.
    window.location.replace(redirectUrl);
  }

  static navigateToReturnUrl(returnUrl: string): void {
    // It's important that we do a replace here so that we remove the callback uri with the
    // fragment containing the tokens from the browser history.
    window.location.replace(returnUrl);
  }

  static getReturnUrl(state: ResultState | null): string {
    const params = new URLSearchParams(window.location.search);
    const fromQuery = params.get(QueryParameterNames.ReturnUrl);
    if (fromQuery && !fromQuery.startsWith(`${window.location.origin}/`)) {
      // This is an extra check to prevent open redirects.
      throw new Error(
        'Invalid return url. The return url needs to have the same origin as the current page.'
      );
    }

    return (
      (state && state.returnUrl) || fromQuery || `${window.location.origin}/`
    );
  }

  constructor(props: Readonly<LoginProps>) {
    super(props);

    this.state = {
      message: undefined,
    };
  }

  componentDidMount(): void {
    const { action } = this.props;
    switch (action) {
      case LoginActions.Login:
        this.login(Login.getReturnUrl(null));
        break;
      case LoginActions.LoginCallback:
        this.processLoginCallback();
        break;
      case LoginActions.LoginFailed:
        this.setLoginFailedState();
        break;
      case LoginActions.Profile:
        Login.redirectToProfile();
        break;
      case LoginActions.Register:
        Login.redirectToRegister();
        break;
      default:
        throw new Error(`Invalid action '${action}'`);
    }
  }

  setLoginFailedState(): void {
    const params = new URLSearchParams(window.location.search);
    const error: string | null = params.get(QueryParameterNames.Message);
    this.setState({ message: error });
  }

  async login(returnUrl: string): Promise<void> {
    const state = { returnUrl };
    const result = await authService.signIn(state);
    switch (result.status) {
      case AuthenticationResultStatus.Redirect:
        break;
      case AuthenticationResultStatus.Success:
        Login.navigateToReturnUrl(returnUrl);
        break;
      case AuthenticationResultStatus.Fail:
        this.setState({ message: result.message });
        break;
      default:
        throw new Error(`Invalid status result ${result.status}.`);
    }
  }

  async processLoginCallback(): Promise<void> {
    const url = window.location.href;
    const result: ResultModel = await authService.completeSignIn(url);
    switch (result.status) {
      case AuthenticationResultStatus.Redirect:
        // There should not be any redirects as the only time completeSignIn finishes
        // is when we are doing a redirect sign in flow.
        throw new Error('Should not redirect.');
      case AuthenticationResultStatus.Success:
        Login.navigateToReturnUrl(Login.getReturnUrl(result.state));
        break;
      case AuthenticationResultStatus.Fail:
        this.setState({ message: result.message });
        break;
      default:
        throw new Error(
          `Invalid authentication result status '${result.status}'.`
        );
    }
  }

  render(): ReactElement {
    const { action } = this.props;
    const { message } = this.state;

    if (message) {
      return <div>{message}</div>;
    }
    switch (action) {
      case LoginActions.Login:
        return <div>Processing login</div>;
      case LoginActions.LoginCallback:
        return <div>Processing login callback</div>;
      case LoginActions.Profile:
      case LoginActions.Register:
        return <div />;
      default:
        throw new Error(`Invalid action '${action}'`);
    }
  }
}

export default Login;
