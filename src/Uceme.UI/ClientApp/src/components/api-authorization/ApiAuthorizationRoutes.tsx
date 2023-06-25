/* eslint-disable react/prefer-stateless-function */
import * as React from 'react';
import { Route, Routes } from 'react-router';
import Login from './Login';
import Logout from './Logout';
import ApplicationPaths, {
  LoginActions,
  LogoutActions,
} from './ApiAuthorizationConstants';

function loginAction(name: string): JSX.Element {
  return <Login action={name} />;
}

function logoutAction(name: string): JSX.Element {
  return <Logout action={name} />;
}

export default class ApiAuthorizationRoutes extends React.Component {
  render(): JSX.Element {
    return (
      <Routes>
        <Route path="/" element={loginAction(LoginActions.Login)} />
        <Route
          path={ApplicationPaths.Login}
          element={loginAction(LoginActions.Login)}
        />
        <Route
          path={ApplicationPaths.LoginFailed}
          element={loginAction(LoginActions.LoginFailed)}
        />
        <Route
          path={ApplicationPaths.LoginCallback}
          element={loginAction(LoginActions.LoginCallback)}
        />
        <Route
          path={ApplicationPaths.Profile}
          element={loginAction(LoginActions.Profile)}
        />
        <Route
          path={ApplicationPaths.Register}
          element={loginAction(LoginActions.Register)}
        />
        <Route
          path={ApplicationPaths.LogOut}
          element={logoutAction(LogoutActions.Logout)}
        />
        <Route
          path={ApplicationPaths.LogOutCallback}
          element={logoutAction(LogoutActions.LogoutCallback)}
        />
        <Route
          path={ApplicationPaths.LoggedOut}
          element={logoutAction(LogoutActions.LoggedOut)}
        />
      </Routes>
    );
  }
}
