import * as React from 'react';
import Nav from 'react-bootstrap/Nav';
import authService from './AuthorizeService';
import ApplicationPaths from './ApiAuthorizationConstants';

type LoginMenuState = {
  userName: string | null | undefined;
  isAuthenticated: boolean;
};

type PathState = {
  local: boolean;
};

type LogoutPath = {
  pathname: string;
  state: PathState;
};

type LoginMenuProps = Record<string, unknown>;

class LoginMenu extends React.Component<LoginMenuProps, LoginMenuState> {
  subscription: number;

  constructor(props: LoginMenuProps) {
    super(props);

    this.state = {
      isAuthenticated: false,
      userName: undefined,
    };

    this.subscription = 0;
  }

  componentDidMount(): void {
    this.subscription = authService.subscribe(() => this.populateState());
    this.populateState();
  }

  componentWillUnmount(): void {
    authService.unsubscribe(this.subscription);
  }

  async populateState(): Promise<void> {
    const [isAuthenticated, user] = await Promise.all([
      authService.isAuthenticated(),
      authService.getUser(),
    ]);
    this.setState({
      isAuthenticated,
      userName: user && user.name,
    });
  }

  static authenticatedView(
    userName: string | undefined,
    profilePath: string | undefined,
    logoutPath: LogoutPath
  ): JSX.Element {
    return (
      <>
        <Nav.Link className="text-dark" href={profilePath}>
          Hola {userName}
        </Nav.Link>
        <Nav.Link className="text-dark" href={logoutPath.pathname}>
          Salir
        </Nav.Link>
      </>
    );
  }

  static anonymousView(
    registerPath: string | undefined,
    loginPath: string | undefined
  ): JSX.Element {
    return (
      <>
        <Nav.Link className="text-dark" href={registerPath}>
          Registro
        </Nav.Link>
        <Nav.Link className="text-dark" href={loginPath}>
          Login
        </Nav.Link>
      </>
    );
  }

  render(): JSX.Element {
    const { isAuthenticated, userName } = this.state;
    if (!isAuthenticated) {
      const registerPath = `${ApplicationPaths.Register}`;
      const loginPath = `${ApplicationPaths.Login}`;
      return LoginMenu.anonymousView(registerPath, loginPath);
    }

    const profilePath = `${ApplicationPaths.Profile}`;
    const logoutPath: LogoutPath = {
      pathname: `${ApplicationPaths.LogOut}`,
      state: { local: true },
    };

    return LoginMenu.authenticatedView(
      userName || undefined,
      profilePath,
      logoutPath
    );
  }
}

export default LoginMenu;
