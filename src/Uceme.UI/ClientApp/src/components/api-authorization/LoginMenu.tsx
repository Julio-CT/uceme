import * as React from 'react';
import Nav from 'react-bootstrap/Nav';
import { ReactElement } from 'react';
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
  static authenticatedView(
    userName: string | undefined,
    profilePath: string | undefined,
    logoutPath: LogoutPath
  ): ReactElement {
    return (
      <>
        <Nav.Link className="text-dark" href="/appointmentmanager">
          Citas
        </Nav.Link>
        <Nav.Link className="text-dark" href="/schedulemanager">
          Horarios
        </Nav.Link>
        <Nav.Link className="text-dark" href="/postmanager">
          Posts
        </Nav.Link>
        <Nav.Link className="text-dark" href="/schedulemanager">
          Horarios
        </Nav.Link>
        <Nav.Link className="text-dark" href={logoutPath.pathname}>
          Salir
        </Nav.Link>
      </>
    );
  }

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
    authService.unsubscribe(this.subscription, 'LoginMenu');
  }

  async populateState(): Promise<void> {
    const [isAuthenticated, user] = await Promise.all([
      authService.isAuthenticated(),
      authService.getUser(),
    ]);
    this.setState({
      isAuthenticated,
      userName: user && user.name?.substring(0, user.name.indexOf('@')),
    });
  }

  render(): ReactElement {
    const { isAuthenticated, userName } = this.state;
    if (!isAuthenticated) {
      return <div />;
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
