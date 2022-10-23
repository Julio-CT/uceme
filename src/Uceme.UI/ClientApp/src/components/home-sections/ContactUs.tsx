/* eslint-disable react/function-component-definition */
import * as React from 'react';
import SettingsContext from '../../SettingsContext';

const ContactUs: () => JSX.Element = () => {
  const settings = React.useContext(SettingsContext);

  if (settings) {
    return (
      <section
        id="section-contact_us"
        className="clearfix container extra-margin"
      >
        <h3 className="uppercase">Contacto</h3>
        <h4 className="padding-y-medium uppercase spacing">
          Estamos a tu disposición para cualquier duda o consulta.
        </h4>
        <div className="line" />
        <p className="subtitulo">
          Contacta por teléfono, por correo electrónico o en persona en nuestras
          instalaciones.
        </p>
        <div className="row justify-content-md-center">
          <div className="contact-item col-12 col-md-4">
            <div className="line-small" />
            <p className="uppercase color-orange">Correo electrónico</p>
            <a
              className="color-gray"
              href="mailto:{settings.ContactEmail}"
              title="email"
            >
              {settings.contactEmail}
            </a>
          </div>
          <div className="contact-item col-12 col-md-4">
            <div className="line-small" />
            <p className="uppercase color-orange">Dirección</p>
            <span className="color-gray">{settings.address}</span>
          </div>
          <div className="contact-item col-12 col-md-4">
            <div className="line-small" />
            <p className="uppercase color-orange">Teléfono</p>
            <a
              className="color-gray"
              title="telefono"
              href="tel:{settings.Telephone}"
            >
              {settings.telephone}
            </a>
          </div>
        </div>
      </section>
    );
  }

  return (
    <section
      id="section-contact_us"
      className="clearfix container extra-margin"
    />
  );
};

export default ContactUs;
