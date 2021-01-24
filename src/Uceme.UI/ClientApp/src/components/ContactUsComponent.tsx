import * as React from 'react';
import './ContactUsComponent.scss';
import SettingsContext from '../SettingsContext';

const ContactUsComponent: () => JSX.Element = () => {
  const settings = React.useContext(SettingsContext());
  const [data, setData] = React.useState<any>({
    loaded: false,
  });

  const handleChange = (event: any) => {
    const name = event.target.name;
    setData({ ...data, [name]: event.target.value });
  }

  const handleSubmit = (event: any) => {
    event.preventDefault();
    alert('A name was submitted: ' + data.name + data.email + data.message);
    fetch(`${settings?.baseHref}api/contact/contactemail`, {
      method: 'POST',
      cache: 'no-cache',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
    }).then(data => {
      console.log(data);
    });
  }

  if (settings) {
    return (
      <section
        id="section-contact_form"
        className="container"
      >
        <div className="App header-distance extra-padding row justify-content-md-center">
          <form onSubmit={handleSubmit} className="col-12 col-md-6">
            <h3 className="uppercase">Envienos un mensaje</h3>
            <h4 className="padding-y-medium uppercase spacing">
              Sus mensajes serán atendidos lo antes posible.
            </h4>
            <div className="line" />
            <label className="contactItem col-9 color-orange">
              Nombre y apellidos*
            </label>
            <input type="text" value={data.name} name='name' onChange={handleChange}  
              className="contactItem col-9"
              placeholder="Nombre y apellidos" 
              autoComplete="nombre" autoCorrect="off" autoCapitalize="none" 
              spellCheck="false" />
            <label className="contactItem col-9 color-orange">
              Dirección de correo electrónico*
            </label>
            <input type="email" value={data.emailAddress} name='email' onChange={handleChange}
             className="contactItem col-9"
             placeholder="Dirección de correo electrónico" 
             autoComplete="email" autoCorrect="off" autoCapitalize="none" 
             spellCheck="false" />
            <label className="contactItem col-9 color-orange">
              Mensaje*
            </label>
            <textarea value={data.message} name='message' onChange={handleChange} 
             className="contactItem col-9"
             placeholder="Escriba su mensaje" 
             autoCorrect="on" autoCapitalize="none" 
             spellCheck="true" />
            <button className="col-9 btn btn-success extra-margin submit-button" type="submit">Enviar</button>
          </form>
          <section
            id="section-contact_us"
            className="col-12 col-md-6"
          >
            <h3 className="uppercase">Contacto</h3>
            <h4 className="padding-y-medium uppercase spacing">
              Estamos a tu disposición para cualquier duda o consulta
            </h4>
            <div className="line" />
            <p className="subtitulo">
              Contacta por teléfono, por correo electrónico o en persona en
              nuestras instalaciones.
            </p>
            <div className="row justify-content-md-center">
              <div className="contactItem col-12">
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
              <div className="contactItem col-12">
                <div className="line-small" />
                <p className="uppercase color-orange">Dirección</p>
                <span className="color-gray">{settings.address}</span>
              </div>
              <div className="contactItem col-12">
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

export default ContactUsComponent;
