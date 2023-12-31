import React, { ReactElement } from 'react';
import { Link, useParams } from 'react-router-dom';
import parse from 'html-react-parser';
import BlogItem from '../library/BlogItem';
import SettingsContext, { Settings } from '../SettingsContext';
import BlogPostResponse from '../library/BlogPostResponse';
import './BlogHome.scss';

type BlogHomeState = {
  loaded: boolean;
  resp?: BlogItem[] | null;
};

function BlogHome(): ReactElement {
  const settings: Settings = React.useContext(SettingsContext);
  const { page } = useParams();
  const dataPageNumber = page ? +page : 1;
  const [data, setData] = React.useState<BlogHomeState>({
    loaded: false,
    resp: null,
  });

  const isFirstRun = React.useRef(true);

  React.useEffect(() => {
    const fetchPosts = (pageNumber: number, baseHref: string) => {
      fetch(`${baseHref}api/blog/getbloglist?page=${pageNumber}`)
        .then((response: Response) => response.json())
        .then(async (resp: BlogPostResponse[]) => {
          const retrievedBlogs: BlogItem[] = [];

          await Promise.all(
            resp.map(async (obj: BlogPostResponse) => {
              const image = `${process.env.PUBLIC_URL}/uploads/${obj.foto.slice(
                obj.foto.lastIndexOf('/') + 1
              )}`;
              retrievedBlogs.push({
                id: obj.idBlog,
                title: obj.titulo,
                imageSrc: image,
                text: obj.texto,
                altText: parse(obj.texto),
                metaDescription: obj.metaDescription,
                caption: obj.titulo,
                link: `/post/${obj.slug}`,
                slug: obj.slug,
                date: new Intl.DateTimeFormat('en-GB', {
                  year: 'numeric',
                  month: 'long',
                  day: '2-digit',
                }).format(new Date(obj.fecha)),
              });
            })
          );

          setData({
            loaded: true,
            resp: retrievedBlogs,
          });
        })
        .catch(() => {
          setData({
            loaded: false,
            resp: null,
          });
        });
    };

    if (settings?.baseHref !== undefined) {
      if (isFirstRun.current) {
        isFirstRun.current = false;
      }

      fetchPosts(dataPageNumber, settings.baseHref);
    }
  }, [dataPageNumber, settings.baseHref]);

  if (data.loaded) {
    const nextPage: number = dataPageNumber ? +dataPageNumber + 1 : 2;
    const previousPage: number | undefined =
      dataPageNumber !== 1 ? +dataPageNumber - 1 : undefined;
    return (
      <div className="app app-home header-distance-l">
        <div className="container" data-testid="blogContainer">
          <div
            className={`section padding-top section-large section-grey section-in-view article-list-container article-list-page-${+dataPageNumber}`}
          >
            {data.resp?.map((post: BlogItem, index: number) => {
              return (
                <React.Fragment key={post.slug}>
                  <article
                    className={`article article-list article-blog article-${
                      +index + 1
                    }`}
                  >
                    <a
                      href={`/post/${post.slug}`}
                      className="article-image article-image-thumb"
                    >
                      <img src={post.imageSrc} alt={post.caption} />
                    </a>
                    <div className="article-inner">
                      <h2 className="article-title">
                        <a href={`/post/${post.slug}`} rel="bookmark">
                          {post.title}
                        </a>
                      </h2>

                      <div className="article-meta">
                        <p className="article-date">{post.date}</p>
                        <p className="article-author">{post.metaDescription}</p>
                      </div>
                    </div>
                  </article>
                </React.Fragment>
              );
            })}
            <div className="navigation-bar">
              <div className="line-small" />
              {previousPage && (
                <Link to={`/blog/${previousPage}`}>{'<<'} Más recientes </Link>
              )}
              {' | '}
              {nextPage && (
                <Link to={`/blog/${nextPage}`}> Anteriores {'>>'} </Link>
              )}
            </div>
          </div>

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
      </div>
    );
  }

  return <div className="app app-home header-distance">Loading...</div>;
}

export default BlogHome;
