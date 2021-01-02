import * as React from 'react';
import { Container } from 'reactstrap';
import './Header.scss';
import NavMenu from './NavMenu';
import logo from '../resources/images/ucemelogobl.png';

const Header = (): JSX.Element => {
  return (
    <header className="uceme-header">
      <Container>
        <div className="col-md-10">
          <a className="container big-logo" href="/" id="headerLogo">
            <img
              alt="logo"
              src={logo}
              width="100%"
              height="100%"
              className="d-inline-block align-top"
            />
          </a>
          <div className="site-title-div hidden-xs" id="headerTitle">
            <h1 className="site-title">
              Unidad de Cirugía Endocrinometabólica Especializada
            </h1>
          </div>
        </div>
        <div className="clearboth" />

        <NavMenu />
      </Container>
    </header>
  );
};

export default Header;
