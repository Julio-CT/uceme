/* eslint-disable @typescript-eslint/explicit-module-boundary-types */
/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable react/jsx-props-no-spreading */
import React from 'react';
import { Navigate } from 'react-router-dom';
import ApplicationPaths, {
  QueryParameterNames,
} from './ApiAuthorizationConstants';
import authService from './AuthorizeService';

type AuthRouteState = {
  isReady: boolean;
  authenticated: boolean;
};

export default class AuthorizeRoute extends React.Component<
  any,
  AuthRouteState
> {
  subscription: number;

  constructor(props: any) {
    super(props);

    this.state = {
      isReady: false,
      authenticated: false,
    };

    this.subscription = 0;
  }

  componentDidMount(): void {
    this.subscription = authService.subscribe(() =>
      this.authenticationChanged()
    );

    this.populateAuthenticationState();
  }

  componentWillUnmount(): void {
    authService.unsubscribe(this.subscription, 'AuthorizeRoute');
  }

  async populateAuthenticationState(): Promise<void> {
    const authenticated = await authService.isAuthenticated();
    this.setState({ isReady: true, authenticated });
  }

  async authenticationChanged(): Promise<void> {
    this.setState({ isReady: false, authenticated: false });
    await this.populateAuthenticationState();
  }

  render() {
    const { isReady, authenticated } = this.state;
    const link = document.createElement('a');
    const { element } = this.props;
    link.href = element.path;
    const returnUrl = `${link.protocol}//${link.host}${link.pathname}${link.search}${link.hash}`;
    const redirectUrl = `${ApplicationPaths.Login}?${
      QueryParameterNames.ReturnUrl
    }=${encodeURIComponent(returnUrl)}`;
    if (!isReady) {
      return <div />;
    }

    return authenticated ? element : <Navigate replace to={redirectUrl} />;
  }
}
