import * as React from 'react';
import { Container } from 'reactstrap';
import './Header.scss';
import { ReactElement } from 'react';
import NavMenu from './NavMenu';
import logo from '../resources/images/logo-uceme-nuevo.webp';
import citaPreviaLogo from '../resources/images/icono-cita-previa.webp';
import AppointmentModal from './appointment-sections/AppointmentModal';

function Header(): ReactElement {
  const [modal, setModal] = React.useState<boolean>(false);
  const toggle: () => void = () => setModal(!modal);

  return (
    <header className="uceme-header">
      <Container>
        <div className="col-md-12">
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
      <div
        className="fixed-button"
        onClick={toggle}
        onKeyDown={toggle}
        role="button"
        tabIndex={0}
      >
        <div className="rounded-fixed-btn">
          <span>Reserva cita</span>
          <img
            alt="citaprevia"
            src={citaPreviaLogo}
            width="100%"
            height="100%"
            className="d-inline-block align-top appointment-icon"
          />
        </div>
      </div>

      <AppointmentModal modal={modal} toggle={toggle} />
    </header>
  );
}

export default Header;
