import * as React from 'react';
import authService, { AuthenticationResultStatus } from './AuthorizeService';

import ApplicationPaths, {
  QueryParameterNames,
  LogoutActions,
  ResultState,
} from './ApiAuthorizationConstants';

// The main responsibility of this component is to handle the user's logout process.
// This is the starting point for the logout process, which is usually initiated when a
// user clicks on the logout button on the LoginMenu component.
type LogoutState = {
  message: string | undefined | null;
  isReady: boolean;
  authenticated: boolean;
};

type LogoutProps = {
  action: string;
};

class Logout extends React.Component<LogoutProps, LogoutState> {
  static navigateToReturnUrl(returnUrl: string): void {
    return window.location.replace(returnUrl);
  }

  componentDidMount(): void {
    const { action } = this.props;
    switch (action) {
      case LogoutActions.Logout:
        this.logout(Logout.getReturnUrl(null));
        break;
      case LogoutActions.LogoutCallback:
        this.processLogoutCallback();
        break;
      case LogoutActions.LoggedOut:
        this.setState({
          isReady: true,
          message: 'You successfully logged out!',
        });
        Logout.navigateToReturnUrl('/');
        break;
      default:
        throw new Error(`Invalid action '${action}'`);
    }

    this.populateAuthenticationState();
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
      (state && state.returnUrl) ||
      fromQuery ||
      `${window.location.origin}${ApplicationPaths.LoggedOut}`
    );
  }

  async populateAuthenticationState(): Promise<void> {
    const authenticated = await authService.isAuthenticated();
    this.setState({ isReady: true, authenticated: authenticated });
  }

  async processLogoutCallback(): Promise<void> {
    const url = window.location.href;
    const result = await authService.completeSignOut(url);
    switch (result.status) {
      case AuthenticationResultStatus.Redirect:
        // There should not be any redirects as the only time completeAuthentication finishes
        // is when we are doing a redirect sign in flow.
        throw new Error('Should not redirect.');
      case AuthenticationResultStatus.Success:
        Logout.navigateToReturnUrl(Logout.getReturnUrl(result.state));
        break;
      case AuthenticationResultStatus.Fail:
        this.setState({ message: result.message });
        break;
      default:
        throw new Error('Invalid authentication result status.');
    }
  }

  async logout(returnUrl: string): Promise<void> {
    const state = { returnUrl };
    const isauthenticated = await authService.isAuthenticated();
    if (isauthenticated) {
      const result = await authService.signOut(state);
      switch (result.status) {
        case AuthenticationResultStatus.Redirect:
          break;
        case AuthenticationResultStatus.Success:
          Logout.navigateToReturnUrl(returnUrl);
          break;
        case AuthenticationResultStatus.Fail:
          this.setState({ message: result.message });
          break;
        default:
          throw new Error('Invalid authentication result status.');
      }
    } else {
      this.setState({ message: 'You successfully logged out!' });
    }
  }

  render(): JSX.Element {
    if (this.state) {
      const { isReady, message } = this.state;
      if (!isReady) {
        return <div>Signing out..</div>;
      }
      if (message) {
        return <div>{message}</div>;
      }
      const { action } = this.props;
      switch (action) {
        case LogoutActions.Logout:
          return <div>Processing logout</div>;
        case LogoutActions.LogoutCallback:
          return <div>Processing logout callback</div>;
        case LogoutActions.LoggedOut:
          return <div>{message}</div>;
        default:
          throw new Error(`Invalid action '${action}'`);
      }
    } else {
      return <div>Signing out...</div>;
    }
  }
}

export default Logout;
