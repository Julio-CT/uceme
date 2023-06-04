import React from 'react';
import { useParams } from 'react-router';
import { Helmet } from 'react-helmet';
import parse from 'html-react-parser';
import BlogItem from '../library/BlogItem';
import BlogPostResponse from '../library/BlogPostResponse';
import SettingsContext, {Settings} from '../SettingsContext';
import './BlogPost.scss';

type BlogPostState = {
  loaded: boolean;
  post?: BlogItem;
};

interface MatchParams {
  slug: string;
}

function BlogPost(): JSX.Element {
  const settings: Settings = React.useContext(SettingsContext);
  const { slug } = useParams<MatchParams>();
  const [data, setData] = React.useState<BlogPostState>({
    loaded: false,
  });

  const fetchPost = (pageSlug: string, baseHref: string) => {
    fetch(`${baseHref}api/blog/getpost?slug=${pageSlug}`)
      .then((response: Response) => response.json())
      .then(async (resp: BlogPostResponse) => {
        const image = `${process.env.PUBLIC_URL}/uploads/${resp.foto.slice(
          resp.foto.lastIndexOf('/') + 1
        )}`;
        const retrievedBlog: BlogItem = {
          id: resp.idBlog,
          title: resp.titulo,
          imageSrc: image,
          text: resp.texto,
          altText: parse(resp.texto),
          metaDescription: resp.metaDescription,
          caption: resp.titulo,
          link: `/post/${resp.slug}`,
          slug: resp.slug,
          seoTitle: resp.seoTitle,
          date: new Intl.DateTimeFormat('en-GB', {
            year: 'numeric',
            month: 'long',
            day: '2-digit',
          }).format(new Date(resp.fecha)),
        };

        setData({
          loaded: true,
          post: retrievedBlog,
        });
      })
      .catch(() => {
        setData({
          loaded: false,
          post: undefined,
        });
      });
  };

  React.useEffect(() => {
    if (settings?.baseHref !== undefined) {
      const pageSlug = slug ?? '';
      fetchPost(pageSlug, settings.baseHref);
    }
  }, [settings.baseHref, slug]);

  if (data.loaded && data.post) {
    return (
      <div className="app-home header-distance-l">
        <div className="container">
          <div className="section padding-top section-large section-grey section-in-view single-article-list-container single-article-list-page-1">
            <Helmet>
              <title>{data.post.seoTitle}</title>
              <meta name="description" content={data.post.metaDescription} />
              <meta name="og:image" content={data.post.featuredImage} />
              <meta property="og:url" content={data.post.link} />
              <meta property="og:type" content="article" />
              <meta property="og:title" content={data.post.title} />
              <meta
                property="og:description"
                content={data.post.metaDescription}
              />
            </Helmet>

            <article className="single-article single-article-list single-article-blog single-article-1">
              <div className="single-article-image single-article-image-thumb">
                <img src={data.post.imageSrc} alt={data.post.caption} />
              </div>
              <div className="single-article-inner">
                <h2 className="single-article-title">
                  <a href={`/post/${data.post.slug}`} rel="bookmark">
                    {data.post.title}
                  </a>
                </h2>
                <div>{data.post.altText}</div>
                <div className="single-article-meta">
                  <p className="single-article-date">{data.post.date}</p>
                  <p className="single-article-author">
                    {data.post.metaDescription}
                  </p>
                </div>
              </div>
            </article>
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

  return <div>Loading...</div>;
}

export default BlogPost;
