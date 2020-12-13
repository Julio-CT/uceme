import * as React from 'react';
import './ContactUs.scss';
import settingsContext, { SettingsContext } from '../../SettingsContext';

const ContactUs = (props: any) => {
    debugger;
    const settings = React.useContext(SettingsContext.instance().context);
    return (
        <section id="section-contact_us" className="clearfix container extra-margin">
            <h3 className="uppercase">Contacto</h3>
            <h4 className="padding-y-medium uppercase spacing">Estamos a tu disposición para cualquier duda o consulta
            </h4>
            <div className="line"></div>
            <p className="subtitulo">Contacta por teléfono, por correo electrónico o en persona en nuestras
                instalaciones.</p>
            <div className='row justify-content-md-center'>
                <div className='contactItem col-12 col-md-4'>
                    <p className="uppercase color-orange">Petición de cita</p>
                    <div className="line-small"></div>
                    <a className="color-gray" title="telefono"
                        href="tel:{settings.Telephone}">{settings.Telephone}</a>
                </div>
                <div className='contactItem col-12 col-md-4'>
                    <p className="uppercase color-orange">Dirección</p>
                    <div className="line-small"></div>
                    <span className="color-gray">{settings.Address}</span>
                </div>
                <div className='contactItem col-12 col-md-4'>
                    <p className="uppercase color-orange">Email</p>
                    <div className="line-small"></div>
                    <a className="color-gray" href="mailto:{settings.ContactEmail}"
                        title="email">{settings.ContactEmail}</a>
                </div>
            </div>
        </section>
    );
}

export default ContactUs;
