/* eslint-disable react/prefer-stateless-function */
import * as React from 'react';
import { Route, Routes } from 'react-router';
import { ReactElement } from 'react';
import Login from './Login';
import Logout from './Logout';
import ApplicationPaths, {
  LoginActions,
  LogoutActions,
} from './ApiAuthorizationConstants';

function loginAction(name: string): ReactElement {
  return <Login action={name} />;
}

function logoutAction(name: string): ReactElement {
  return <Logout action={name} />;
}

export default class ApiAuthorizationRoutes extends React.Component {
  render(): ReactElement {
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
