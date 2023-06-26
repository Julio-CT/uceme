/* eslint-disable react/function-component-definition */
import * as React from 'react';
import SettingsContext, { Settings } from '../SettingsContext';
import './AboutUs.scss';
import alberto from '../resources/images/alberto.webp';
import amunategui from '../resources/images/amunategui.webp';
import bettina from '../resources/images/bettina.webp';
import blanca from '../resources/images/blanca.webp';
import atocha from '../resources/images/atocha.webp';
import maria from '../resources/images/maria.webp';
import mercader from '../resources/images/mercader.webp';

const specials = [
  {
    title: 'amunategui',
    src: amunategui,
    altText:
      'Cirugia general y del aparato digestivo. Especialidad cirugia endocrina.',
    caption: 'Dr. Iñaki Amunategui',
    link: 'amunategui',
    owner: true,
  },
  {
    title: 'mercader',
    src: mercader,
    altText:
      'Cirugia general y del aparato digestivo. Especialidad cirugia endocrina.',
    caption: 'Dr. Enrique Mercader',
    link: 'mercader',
    owner: true,
  },
  {
    title: 'alberto',
    src: alberto,
    altText: 'Endocrinología y nutrición.',
    caption: 'Dr. Alberto Sanchez',
    link: 'alberto',
    owner: false,
  },
  {
    title: 'bettina',
    src: bettina,
    altText: 'Endocrinología y nutrición.',
    caption: 'Dra. Bettina Weber',
    link: 'suprarrenales',
    owner: false,
  },
  {
    title: 'maria',
    src: maria,
    altText: 'Endocrinología y nutrición.',
    caption: 'Dra. Maria Requena',
    link: 'maria',
    owner: false,
  },
  {
    title: 'atocha',
    src: atocha,
    altText: 'Nutrición y dietética.',
    caption: 'Dra. Atocha Bielza',
    link: 'atocha',
    owner: false,
  },
  {
    title: 'blanca',
    src: blanca,
    altText: 'Secretaria.',
    caption: 'Blanca Goded',
    link: 'blanca',
    owner: false,
  },
];

const items: JSX.Element[] = specials.map((item) => {
  return (
    <div
      className={`col-12 ${item.owner ? 'col-md-6' : 'col-md-4'}`}
      key={item.title}
    >
      <img
        src={item.src}
        alt={item.title}
        title={item.title}
        className="round-photo"
      />
      <h4 className="uppercase">
        <a href={`/quienessomos/${item.link}`} title={item.title}>
          {item.caption}
        </a>
      </h4>
      <div className="line-small" />
      <p>
        <a href={`/quienessomos/${item.link}`} title={item.title}>
          {item.altText}
        </a>
      </p>
    </div>
  );
});

const AboutUs: () => JSX.Element = () => {
  const settings: Settings = React.useContext(SettingsContext);
  return (
    <div className="app app-home header-distance">
      <section id="section-about-us" className="header-distance">
        <div className="about-us container">
          <h3 className="uppercase">Nuestro Equipo</h3>
          <h4 className="padding-y-medium spacing uppercase">
            Un excelente grupo de profesionales.
          </h4>
          <div className="line" />
          <div className="row justify-content-md-center">{items}</div>
        </div>
      </section>
      <section id="section-about-us" className="header-distance">
        <div className="about-us container">
          <p className="padding-y-medium spacing">
            En las últimas décadas, los avances en el campo de la Cirugía han
            sido muy importantes. El volumen de conocimientos necesario para ser
            experto en un área concreta es cada día más notable. Por otro lado,
            el papel de la tecnología es progresivamente más relevante, lo que
            nos permite ofrecer mayor seguridad a nuestros pacientes.
          </p>
          <p className="padding-y-medium spacing">
            Con el objetivo de formar un equipo de excelencia, altamente
            especializado en el área de la Cirugía Endocrina y en el tratamiento
            de la Obesidad, surgió este grupo de Cirujanos que forma la Unidad
            de Cirugía Endocrino-Metabólica Especializada (UCEME Madrid). En
            UCEME ponemos al servicio de profesionales y pacientes la
            experiencia acumulada durante años de trabajo.
          </p>
          <p className="padding-y-medium spacing">
            Hemos tratado de aunar los mejores conocimientos adquiridos con la
            experiencia con la seguridad que nos proporciona la tecnología
            actual: dispositivos modernos de coagulación que minimizan la
            pérdida hemática, empleo de la cirugía laparoscópica como
            herramienta de escasa agresión y alta resolución, monitorización
            nerviosa intraoperatoria para minimizar el riesgo de los nervios
            laríngeos, etc.
          </p>
          <p className="padding-y-medium spacing">
            Con todo ello, y contando con un gran grupo de profesionales de
            distintas especialidades, ofrecemos una medicina de calidad y
            seguridad que proporciona una cobertura completa y satisfactoria en
            todos los aspectos relacionados con nuestro campo de actuación:
            Tratamiento de la patología del tiroides por radiofrecuencia o por
            cirugía, con o sin incisión visible en el cuello. Cirugía de las
            glándulas paratiroides y suprarrenales, así como un tratamiento
            personalizado de la obesidad.
          </p>
          <p className="padding-y-medium spacing">
            Ofrecemos un trato personalizado en enfermedades de tiroides,
            paratiroides, glándula suprarrenal y obesidad mórbida.
          </p>
        </div>
      </section>
      <section
        id="section-contact_us"
        className="clearfix container extra-margin app"
      >
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
    </div>
  );
};

export default AboutUs;
