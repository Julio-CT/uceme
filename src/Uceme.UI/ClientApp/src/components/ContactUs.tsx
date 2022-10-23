/* eslint-disable react/function-component-definition */
import * as React from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button } from 'reactstrap';
import { useHistory } from 'react-router-dom';
import SettingsContext from '../SettingsContext';
import './ContactUs.scss';

type contactUsState = {
  loaded: boolean;
  name: string;
  email: string;
  message: string;
};

const ContactUs: () => JSX.Element = () => {
  const [modal, setModal] = React.useState(false);
  const toggle = () => setModal(!modal);
  const [modalMessage, setModaleMessage] = React.useState<string>('');
  const history = useHistory();
  const settings = React.useContext(SettingsContext);
  const [data, setData] = React.useState<contactUsState>({
    loaded: false,
    name: '',
    email: '',
    message: '',
  });

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name } = event.target;
    setData({ ...data, [name]: event.target.value });
  };

  const handleChangeText = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    const { name } = event.target;
    setData({ ...data, [name]: event.target.value });
  };

  const showAlert: (message: string) => void = (message: string) => {
    setModaleMessage(message);
    setModal(true);
  };

  const closeAlert: () => void = () => {
    toggle();
    history.push('/#home');
  };

  const handleSubmit: (event: React.FormEvent) => void = (
    event: React.FormEvent
  ) => {
    event.preventDefault();
    fetch(`${settings?.baseHref}api/contact/contactemail`, {
      method: 'POST',
      cache: 'no-cache',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    }).then((response) => {
      if (response && response.status === 200) {
        showAlert(
          'Correo electrónico enviado. Nuestro equipo se pondrá en contacto lo antes posible. Muchas gracias.'
        );

        setData({
          loaded: true,
          name: '',
          email: '',
          message: '',
        });
      } else {
        showAlert(
          'Lo sentimos, el envío del correo electrónico ha fallado, por favor inténtelo en unos minutos.'
        );
      }
    });
  };

  if (settings) {
    return (
      <section id="section-contact_form" className="container">
        <Modal isOpen={modal} toggle={toggle} className="next-dates-modal">
          <ModalHeader toggle={toggle} className="beatabg next-dates-modal">
            <div className="aligner next-dates-modal">
              <div className="aligner-item aligner-item-top" />
              <div className="aligner-item" />
              <div className="aligner-item aligner-item-bottom" />
            </div>
          </ModalHeader>
          <ModalBody>
            <section id="section-contact_form" className="container">
              <div className="row justify-content-md-center">
                {modalMessage}
              </div>
            </section>
          </ModalBody>
          <ModalFooter>
            {' '}
            <Button color="secondary" onClick={closeAlert}>
              Cerrar
            </Button>
          </ModalFooter>
        </Modal>
        <div className="app header-distance extra-padding row justify-content-md-center">
          <form onSubmit={handleSubmit} className="col-12 col-md-6">
            <h3 className="uppercase">Envíanos un mensaje</h3>
            <h4 className="padding-y-medium uppercase spacing">
              Tu mensaje será atendido lo antes posible.
            </h4>
            <div className="line" />
            <label htmlFor="name" className="contact-item col-9 color-orange">
              Nombre y apellidos:
              <input
                id="name"
                type="text"
                value={data.name}
                name="name"
                onChange={handleChange}
                className="contact-input col-12"
                placeholder="Nombre y apellidos"
                autoComplete="nombre"
                autoCorrect="off"
                autoCapitalize="none"
                spellCheck="false"
              />
            </label>
            <label htmlFor="email" className="contact-item col-9 color-orange">
              Dirección de correo electrónico:
              <input
                id="email"
                type="email"
                value={data.email}
                name="email"
                onChange={handleChange}
                className="contact-input col-12"
                placeholder="Dirección de correo electrónico"
                autoComplete="email"
                autoCorrect="off"
                autoCapitalize="none"
                spellCheck="false"
              />
            </label>
            <label
              htmlFor="message"
              className="contact-item col-9 color-orange"
            >
              Mensaje:
              <textarea
                id="message"
                value={data.message}
                name="message"
                onChange={handleChangeText}
                className="contact-input col-12"
                placeholder="Escriba su mensaje"
                autoCorrect="on"
                autoCapitalize="none"
                spellCheck="true"
              />
            </label>
            <button
              className="col-9 btn btn-success extra-margin submit-button contact-input contact-button"
              type="submit"
            >
              Enviar
            </button>
          </form>
          <section id="section-contact_us" className="col-12 col-md-6">
            <h3 className="uppercase">Contacto</h3>
            <h4 className="padding-y-medium uppercase spacing">
              Estamos a tu disposición para cualquier duda o consulta.
            </h4>
            <div className="line" />
            <p className="subtitulo">
              Contacta por teléfono, por correo electrónico o en persona en
              nuestras instalaciones.
            </p>
            <div className="row justify-content-md-center">
              <div className="contact-item col-12">
                <div className="line-small" />
                <p className="uppercase color-orange">Correo electrónico</p>
                <a
                  className="color-gray"
                  href={`mailto:${settings.contactEmail}`}
                  title="email"
                >
                  {settings.contactEmail}
                </a>
              </div>
              <div className="contact-item col-12">
                <div className="line-small" />
                <p className="uppercase color-orange">Dirección</p>
                <span className="color-gray">{settings.address}</span>
              </div>
              <div className="contact-item col-12">
                <div className="line-small" />
                <p className="uppercase color-orange">Teléfono</p>
                <a
                  className="color-gray"
                  title="telefono"
                  href={`tel:${settings.telephone}`}
                >
                  {settings.telephone}
                </a>
                <p className="color-gray">
                  Horario de atención:
                  <br /> L 9:00 a 14:00
                  <br /> M, X, J 15:30 a 20:00
                </p>
              </div>
            </div>
          </section>
        </div>
      </section>
    );
  }

  return (
    <section
      id="section-contact_us"
      className="clearfix container extra-margin"
    >
      Loading...
    </section>
  );
};

export default ContactUs;
