import React from 'react';
import { useParams } from 'react-router';
import { Helmet } from 'react-helmet';
import parse from 'html-react-parser';
import SettingsContext from '../SettingsContext';
import slide1 from '../resources/images/especialidad1.jpg';
import slide2 from '../resources/images/especialidad2.jpg';
import slide3 from '../resources/images/especialidad3.jpg';
import slide4 from '../resources/images/especialidad4.jpg';
import slide5 from '../resources/images/especialidad5.jpg';
import './Specialities.scss';

const specials: Special[] = [
  {
    title: 'cirugia',
    img: slide1,
    src: 'Los procedimientos quirúrgicos sobre el tiroides pueden estar indicados en diferentes situaciones; nódulos tiroideos, grandes bocios con crecimiento del tiroides hacia la cavidad torácica, cáncer de tiroides o hipertiroidismo no controlado con medicación. <br/> <br/>En UCEME proponemos un abordaje individualizado del problema con la posibilidad de realizar resección parcial o total del tiroides, así como de las cadenas ganglionares cervicales, según precise cada caso.',
    caption: 'Cirugía Tiroidea',
  },
  {
    title: 'paratiroidea',
    img: slide2,
    src: 'El hiperparatiroidismo primario consiste en una secreción aumentada de la paratohormona por parte de las glándulas paratiroides. Su tratamiento es quirúrgico, y consiste en la extirpación de la glándula o las glándulas causantes de dicho exceso de secreción.<br/> <br/>La experiencia del equipo quirúrgico es especialmente importante en esta patología para que la intervención quirúrgica sea resolutiva. En UCEME realizamos más de 80 procedimientos anuales de patología paratiroidea con excelentes resultados en un alto porcentaje de los casos.',
    caption: 'Cirugía Paratiroidea',
  },
  {
    title: 'obesidad',
    img: slide3,
    src: 'Las glándulas adrenales se sitúan por encima del polo superior de ambos riñones y pueden presentar nódulos en su interior que asocien exceso en la secreción de determinadas hormonas. Su tratamiento suele ser quirúrgico, precisando extirpaciones parciales o completas de la glándula.<br/> <br/>En UCEME proponemos un abordaje multidisciplinar médico-quirúrgico con el fin de ajustar el estudio y preparación preoperatoria, así como la técnica quirúrgica y el manejo postoperatorio más adecuado en cada caso.',
    caption: 'Obesidad mórbida',
  },
  {
    title: 'suprarrenales',
    img: slide4,
    src: 'La obesidad es una enfermedad muy frecuente en nuestra sociedad, que precisa de una atención especializada por equipos compuestos por médicos endocrinólogos y nutricionistas expertos en nutrición. Algunos de estos casos precisarán una intervención quirúrgica.<br/> <br/>En UCEME ofrecemos un equipo multidisciplinar que atienda al paciente en todos los aspectos de la enfermedad, incluyendo las diferentes técnicas quirúrgicas al alcance en el tratamiento de la obesidad',
    caption: 'Cirugía de Glándulas Suprarrenales',
  },
  {
    title: 'tiroidectomia',
    img: slide5,
    src: 'En los últimos años se han desarrollado diferentes técnicas para el tratamiento quirúrgico de la patología tiroidea tratando de evitar la cicatriz cervical.<br/> En UCEME comenzamos a realizar este abordaje en 2017 habiendo sido pioneros en Europa del abordaje Biaxilo-Biareolar, por lo que ofrecemos una amplia experiencia en este campo.',
    caption: 'Tiroidectomía sin cicatriz',
  },
];

function fetchData(title: string): Promise<Special> {
  let result: Special;
  specials.forEach((element) => {
    if (element.title === title) {
      result = element;
    }
  });

  return new Promise((resolve) => {
    resolve(result);
  });
}

type SpecialityState = {
  loaded: boolean;
  speciality?: Special;
};

type Special = {
  title: string;
  img: string;
  src: string;
  caption: string;
};

interface MatchParams {
  esp: string;
}

function Speciality(): JSX.Element {
  const settings = React.useContext(SettingsContext());
  const { esp } = useParams<MatchParams>();
  const [data, setData] = React.useState<SpecialityState>({
    loaded: false,
  });

  const fetchPost = (slug: string) => {
    fetchData(slug)
      .then((response: Special) => response)
      .then(async (resp: Special) => {
        setData({
          loaded: true,
          speciality: resp,
        });
      })
      .catch(() => {
        setData({
          loaded: false,
          speciality: undefined,
        });
      });
  };

  React.useEffect(() => {
    if (settings) {
      const slug = esp ?? '';
      fetchPost(slug);
    }
  }, [esp, settings]);

  if (data.loaded && data.speciality) {
    return (
      <div className="app-home header-distance header-distance-l">
        <div className="container">
          <div className="section padding-top section-large section-grey section-in-view article-list-container article-list-page-1">
            <Helmet>
              <title>{data.speciality.title.toUpperCase()}</title>
              <meta name="description" content={data.speciality.caption} />
              <meta property="og:url" content={data.speciality.caption} />
              <meta property="og:type" content="article" />
              <meta property="og:title" content={data.speciality.title} />
              <meta
                property="og:description"
                content={data.speciality.caption}
              />
            </Helmet>

            <article className="article article-list article-blog article-1">
              <a
                href={`/especialidad/${data.speciality.title}`}
                className="article-image article-image-thumb"
              >
                <img src={data.speciality.img} alt={data.speciality.caption} />
              </a>
              <div className="article-inner">
                <h2 className="article-title">{data.speciality.caption}</h2>
                <div>{parse(data.speciality.src)}</div>
              </div>
              <a
                href="/especialidades"
                className="uppercase color-orange app all-especialities"
              >
                Todas las especialidades
              </a>
            </article>
          </div>
        </div>
        <section
          id="section-contact_us"
          className="clearfix container extra-margin app"
        >
          <div className="row justify-content-md-center">
            <div className="contactItem col-12 col-md-4">
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
            <div className="contactItem col-12 col-md-4">
              <div className="line-small" />
              <p className="uppercase color-orange">Dirección</p>
              <span className="color-gray">{settings.address}</span>
            </div>
            <div className="contactItem col-12 col-md-4">
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
  }

  return <div>Loading...</div>;
}

export default Speciality;
