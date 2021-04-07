import * as React from 'react';
import { Container } from 'reactstrap';
import './Header.scss';
import NavMenu from './NavMenu';
import logo from '../resources/images/ucemelogobl.png';
import citaPreviaLogo from '../resources/images/icono-cita-previa.webp';
import AppointmentModal from './AppointmentModal';

const Header = (): JSX.Element => {
  const [modal, setModal] = React.useState(false);
  const toggle = () => setModal(!modal);

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
      <div
        className="fixedButton"
        onClick={toggle}
        onKeyDown={toggle}
        role="button"
        tabIndex={0}
      >
        <div className="roundedFixedBtn">
          <span>Reserve cita</span>
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
};

export default Header;
