import * as React from 'react'
import { Component } from 'react';
import Navbar from 'react-bootstrap/Navbar';
import Nav from 'react-bootstrap/Nav'
import { Container } from 'reactstrap';
import { LoginMenu } from './api-authorization/LoginMenu';
import './NavMenu.css';
import logo from '../resources/images/ucemelogobl.png';

type NavMenuState = {
    collapsed: boolean,
}

type NavMenuProps = {
}

export class NavMenu extends Component<NavMenuProps, NavMenuState>  {
    static displayName = NavMenu.name;

    constructor(props: NavMenuProps) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render() {
        return (
            <header className="uceme-header">
                <Container>
                    <div>
                        <div className="col-md-10">
                            <a className="container big-logo" href="/" id="headerLogo">
                            <img
                                alt="logo"
                                src={ logo }
                                width="100%"
                                height="100%"
                                className="d-inline-block align-top"
                            />
                                <meta itemProp="image" content="http://www.endocrinologia-madrid.com/Images/ucemelogobl.png" />
                            </a>
                            <div className="site-title-div hidden-xs" id="headerTitle">
                                <h1 className="site-title">
                                    Unidad de Cirugía Endocrinometabólica Especializada
                                </h1>
                            </div>
                        </div>
                    </div>
                    <div className="clearboth"></div>

                    <Navbar className="bg-light">
                        <Navbar.Toggle aria-controls="basic-navbar-nav" />
                        <Navbar.Collapse id="basic-navbar-nav">
                            <Nav className="mr-auto">
                                <Navbar.Brand href="#home">Somos Uceme</Navbar.Brand>
                                <Nav.Link href="/especialidades">Especialidades</Nav.Link>
                                <Nav.Link href="/innovaciones">Innovaciones Técnicas</Nav.Link>
                                <Nav.Link href="/blog">Blog</Nav.Link>
                                <Nav.Link href="/contacto">Contacto</Nav.Link>
                                <Nav.Link href="/counter">Counter</Nav.Link>
                                <Nav.Link href="/fetch-data">Fetch data</Nav.Link>
                                <LoginMenu>
                                </LoginMenu>
                            </Nav>
                        </Navbar.Collapse>
                    </Navbar>
                </Container>
            </header>
        );
    }
}
