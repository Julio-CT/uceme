import * as React from 'react'
import { Fragment } from 'react';
import authService from './AuthorizeService';
import ApplicationPaths from './ApiAuthorizationConstants';
import Nav from 'react-bootstrap/Nav';

type LoginMenuState = {
    userName: any,
    isAuthenticated: boolean,
}

type LoginMenuProps = {
}

export class LoginMenu extends React.Component<LoginMenuProps, LoginMenuState> {
    _subscription: number;
    constructor(props: LoginMenuProps) {
        super(props);

        this.state = {
            isAuthenticated: false,
            userName: null
        };

        this._subscription = 0;
    }

    componentDidMount(): void {
        this._subscription = authService.subscribe(() => this.populateState());
        this.populateState();
    }

    componentWillUnmount(): void {
        authService.unsubscribe(this._subscription);
    }

    async populateState(): Promise<void> {
        const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()]);
        this.setState({
            isAuthenticated,
            userName: user && user.name
        });
    }

    authenticatedView(userName: any, profilePath: any, logoutPath: any): JSX.Element {
        console.log("userName");
        console.log(userName);
        console.log("profilePath");
        console.log(profilePath);
        console.log("logoutPath");
        console.log(logoutPath);
        return (<Fragment>
            <Nav.Link className="text-dark" href={profilePath}>Hola {userName}</Nav.Link>
            <Nav.Link className="text-dark" href={logoutPath.pathname}>Salir</Nav.Link>
        </Fragment>);
    }

    anonymousView(registerPath: any, loginPath: any): JSX.Element {
        console.log("registerPath");
        console.log(registerPath);
        console.log("loginPath");
        console.log(loginPath);
        return (<Fragment>
            <Nav.Link className="text-dark" href={registerPath}>Registro</Nav.Link>
            <Nav.Link className="text-dark" href={loginPath}>Login</Nav.Link>
        </Fragment>);
    }

    render(): JSX.Element {
        const { isAuthenticated, userName } = this.state;
        if (!isAuthenticated) {
            const registerPath = `${ApplicationPaths.Register}`;
            const loginPath = `${ApplicationPaths.Login}`;
            return this.anonymousView(registerPath, loginPath);
        } else {
            const profilePath = `${ApplicationPaths.Profile}`;
            const logoutPath = { pathname: `${ApplicationPaths.LogOut}`, state: { local: true } };
            return this.authenticatedView(userName, profilePath, logoutPath);
        }
    }
}
