import React from 'react';
import { useParams } from 'react-router';
import { Helmet } from 'react-helmet';
import parse from 'html-react-parser';
import SettingsContext from '../SettingsContext';
import TechniqueResponse from '../library/TechniqueResponse';
import './Technique.scss';

type TechniqueState = {
  technique?: TechniqueResponse;
  loaded: boolean;
};

interface MatchParams {
  tec: string;
}

function Technique(): JSX.Element {
  const settings = React.useContext(SettingsContext);
  const { tec } = useParams<MatchParams>();

  const [data, setData] = React.useState<TechniqueState>({
    loaded: false,
  });

  const fetchPost = (techinqueId: string, baseHref: string) => {
    fetch(`${baseHref}api/technique/gettechnique?techinqueId=${techinqueId}`)
      .then((response: Response) => response.json())
      .then(async (resp: TechniqueResponse) => {
        setData({
          loaded: true,
          technique: {
            idTecnica: resp.idTecnica,
            titulo: resp.titulo,
            texto: resp.texto,
            foto: resp.foto.replace('~/', '').replace('/fotos', ''),
            nombre: resp.nombre,
          } as TechniqueResponse,
        });
      })
      .catch(() => {
        setData({
          loaded: false,
          technique: undefined,
        });
      });
  };

  React.useEffect(() => {
    if (settings?.baseHref !== undefined) {
      const pageSlug = tec ?? '3';
      fetchPost(pageSlug, settings.baseHref);
    }
  }, [tec, settings.baseHref]);

  if (data.loaded && data.technique) {
    return (
      <div className="app-home header-distance">
        <div className="container">
          <div className="section padding-top section-large section-grey section-in-view article-list-container article-list-page-1">
            <Helmet>
              <title>{data.technique.titulo.toUpperCase()}</title>
              <meta name="description" content={data.technique.titulo} />
              <meta property="og:url" content={data.technique.idTecnica} />
              <meta property="og:type" content="article" />
              <meta property="og:title" content={data.technique.titulo} />
              <meta property="og:description" content={data.technique.titulo} />
            </Helmet>

            <article className="article article-list article-blog article-1">
              <a
                href={`/tecnica/${data.technique.titulo}`}
                className="article-image article-image-thumb"
              >
                <img src={data.technique.foto} alt={data.technique.nombre} />
              </a>
              <div className="article-inner">
                <h2 className="article-title">{data.technique.nombre}</h2>
                <div>{parse(data.technique.texto)}</div>
              </div>
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

export default Technique;
