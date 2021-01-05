import * as React from 'react';
import './SpecialitiesComponent.scss';
import slide1 from '../resources/images/cirugia-tiroidea.png';
import slide2 from '../resources/images/cirugia-paratiroidea.png';
import slide3 from '../resources/images/obesidad-morbida.png';
import slide4 from '../resources/images/cirugia-glandulas-suprarrenales.png';
import slide5 from '../resources/images/tiroidectomia-sin-cicatriz.png';

const specials = [
  {
    title: 'cirugia',
    src: slide1,
    altText: 'Unidad de Cirugia Endocrinometabolica Especializada',
    caption: 'Cirugía Tiroidea',
    link: 'Cirugía de Glándulas Suprarrenales',
  },
  {
    title: 'Paratiroidea',
    src: slide2,
    altText: 'Pioneros en España en Tiroidectomía por Abordaje Extracervical',
    caption: 'Cirugía Paratiroidea',
    link: 'Cirugía de Glándulas Suprarrenales',
  },
  {
    title: 'Obesidad',
    src: slide3,
    altText:
      'Ofrecemos un trato personalizado en enfermedades de tiroides, paratiroides, glándula suprarrenal y obesidad mórbida.',
    caption: 'Obesidad mórbida',
    link: 'Cirugía de Glándulas Suprarrenales',
  },
  {
    title: 'Suprarrenales',
    src: slide4,
    altText: 'Sistema de cita previa online',
    caption: 'Cirugía de Glándulas Suprarrenales',
    link: 'Cirugía de Glándulas Suprarrenales',
  },
  {
    title: 'tiroidectomia',
    src: slide5,
    altText: 'Sistema de cita previa online',
    caption: 'Tiroidectomía sin cicatriz',
    link: 'Cirugía de Glándulas Suprarrenales',
  },
];

const items: JSX.Element[] = specials.map((item) => {
  return (
    <div className="col-12 col-md-4" key={item.title}>
      <a href={item.link} title={item.title}>
        <img
          src={item.src}
          className="attachment- size-"
          alt={item.title}
          title={item.title}
          width="120"
          height="80"
        />
      </a>
      <h4 className="uppercase">
        <a href={item.link} title={item.title}>
          {item.caption}
        </a>
      </h4>
      <div className="line-small" />
      <p>
        <a href={item.link} title={item.title}>
          {item.altText}
        </a>
      </p>
    </div>
  );
});

const SpecialitiesComponent: () => JSX.Element = () => {
  return (
    <div className="App App-home header-distance">
      <section id="section-specialities" className="clearfix">
        <div className="specialities container clearfix extra-margin">
          <h3 className="uppercase">Especialidades</h3>
          <h4 className="padding-y-medium spacing uppercase">
            La última tecnología unida a un excelente trato personal
          </h4>
          <div className="line" />
          <div className="row justify-content-md-center">{items}</div>
        </div>
      </section>
    </div>
  );
};

export default SpecialitiesComponent;
