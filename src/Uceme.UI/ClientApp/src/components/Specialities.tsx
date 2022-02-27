/* eslint-disable react/function-component-definition */
import * as React from 'react';
import './Specialities.scss';
import slide1 from '../resources/images/cirugia-tiroidea.png';
import slide2 from '../resources/images/cirugia-paratiroidea.png';
import slide3 from '../resources/images/obesidad-morbida.png';
import slide4 from '../resources/images/cirugia-glandulas-suprarrenales.png';
import slide5 from '../resources/images/tiroidectomia-sin-cicatriz.png';

const specials = [
  {
    title: 'cirugia',
    src: slide1,
    altText:
      'Abordaje de bocio multinodular, cáncer de tiroides e hipertiroidismo, entre otras.',
    caption: 'Cirugía Tiroidea',
    link: 'cirugia',
  },
  {
    title: 'paratiroidea',
    src: slide2,
    altText: 'Técnicas mínimamente invasivas.',
    caption: 'Cirugía Paratiroidea',
    link: 'paratiroidea',
  },
  {
    title: 'obesidad',
    src: slide3,
    altText:
      'Amplia experiencia en cirugía laparoscópica con técnicas personalizadas a cada paciente.',
    caption: 'Obesidad mórbida',
    link: 'obesidad',
  },
  {
    title: 'suprarrenales',
    src: slide4,
    altText:
      'Abordaje multidisciplinar con estudio funcional completo y cirugía mínimamente invasiva.',
    caption: 'Cirugía de Glándulas Suprarrenales',
    link: 'suprarrenales',
  },
  {
    title: 'tiroidectomia',
    src: slide5,
    altText:
      'Abordaje multidisciplinar con estudio funcional completo y cirugía mínimamente invasiva.',
    caption: 'Tiroidectomía sin cicatriz',
    link: 'tiroidectomia',
  },
];

const items: JSX.Element[] = specials.map((item) => {
  return (
    <div className="col-12 col-md-4" key={item.title}>
      <a href={`/especialidad/${item.link}`} title={item.title}>
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
        <a href={`/especialidad/${item.link}`} title={item.title}>
          {item.caption}
        </a>
      </h4>
      <div className="line-small" />
      <p>
        <a href={`/especialidad/${item.link}`} title={item.title}>
          {item.altText}
        </a>
      </p>
    </div>
  );
});

const Specialities: () => JSX.Element = () => {
  return (
    <div className="app app-home header-distance header-distance-l">
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

export default Specialities;
