import React from 'react';
import { useParams } from 'react-router';
import { Helmet } from 'react-helmet';
import parse from 'html-react-parser';
import BlogItem from '../library/BlogItem';
import BlogPostResponse from '../library/BlogPostResponse';
import SettingsContext from '../SettingsContext';
import './BlogHome.scss';

type BlogPostState = {
  loaded: boolean;
  post?: BlogItem;
};

interface MatchParams {
  slug: string;
}

function BlogPost(): JSX.Element {
  const settings = React.useContext(SettingsContext());
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
    if (settings) {
      const pageSlug = slug ?? '';
      fetchPost(pageSlug, settings.baseHref);
    }
  }, [settings, slug]);

  if (data.loaded && data.post) {
    return (
      <div className="app-home header-distance">
        <div className="container">
          <div className="section padding-top section-large section-grey section-in-view article-list-container article-list-page-1">
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

            <article className="article article-list article-blog article-1">
              <a
                href={`/post/${data.post.slug}`}
                className="article-image article-image-thumb"
              >
                <img src={data.post.imageSrc} alt={data.post.caption} />
              </a>
              <div className="article-inner">
                <h2 className="article-title">
                  <a href={`/post/${data.post.slug}`} rel="bookmark">
                    {data.post.title}
                  </a>
                </h2>
                <div>{data.post.altText}</div>
                <div className="article-meta">
                  <p className="article-date">{data.post.date}</p>
                  <p className="article-author">{data.post.metaDescription}</p>
                </div>
              </div>
            </article>
          </div>
        </div>
      </div>
    );
  }

  return <div>Loading...</div>;
}

export default BlogPost;
