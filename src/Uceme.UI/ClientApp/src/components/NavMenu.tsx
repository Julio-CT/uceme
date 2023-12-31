import * as React from 'react';
import Navbar from 'react-bootstrap/Navbar';
import Nav from 'react-bootstrap/Nav';
import './NavMenu.scss';
import { ReactElement } from 'react';
import LoginMenu from './api-authorization/LoginMenu';

type NavMenuState = {
  collapsed: boolean;
};

type NavMenuProps = Record<string, unknown>;

export default class NavMenu extends React.Component<
  NavMenuProps,
  NavMenuState
> {
  constructor(props: NavMenuProps) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
    };
  }

  toggleNavbar(): void {
    this.setState((prevState) => ({ collapsed: !prevState.collapsed }));
  }

  render(): ReactElement {
    return (
      <div className="navbar-div-container">
        <Navbar collapseOnSelect expand="lg" className="bg-light">
          <Navbar.Toggle aria-controls="responsive-navbar-nav" />
          <Navbar.Collapse id="responsive-navbar-nav">
            <Nav className="mr-auto">
              <Navbar.Brand href="#home">Somos Uceme</Navbar.Brand>
              <Nav.Link href="/especialidades">Especialidades</Nav.Link>
              <Nav.Link href="/tecnica/3">Innovaciones Técnicas</Nav.Link>
              <Nav.Link href="/quienessomos">Nuestro Equipo</Nav.Link>
              <Nav.Link href="/blog">Blog</Nav.Link>
              <Nav.Link href="/contacto">Contacto</Nav.Link>
            </Nav>
            <Nav>
              <LoginMenu />
            </Nav>
          </Navbar.Collapse>
        </Navbar>
      </div>
    );
  }
}
