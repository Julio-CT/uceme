import React from 'react';
import { Helmet } from 'react-helmet';
import BlogPost from '../library/BlogPost';
import parse from 'html-react-parser';
import SettingsContext from '../SettingsContext';
import './BlogHome.scss';

type BlogPostState = {
  loaded: boolean;
  post?: BlogPost;
};

type BlogPostProps = {
  children: React.ReactElement[];
  params?: any;
  history?: any;
  location?: any;
  match?: any;
};

const BlogPostComponent = (props: BlogPostProps) => {
  const settings = React.useContext(SettingsContext());
  const [data, setData] = React.useState<BlogPostState>({
    loaded: false,
  });

  const fetchPosts = (slug: string, baseHref: string) => {
    fetch(`${baseHref}api/blog/getpost?slug=${slug}`)
      .then((response: { json: () => any }) => response.json())
      .then(async (resp: any) => {
        let retrievedBlog: BlogPost;
        const image =
          process.env.PUBLIC_URL +
          '/uploads/' +
          resp.foto.slice(resp.foto.lastIndexOf('/') + 1);
        retrievedBlog = {
          id: resp.idBlog,
          title: resp.titulo,
          imageSrc: image,
          text: resp.texto,
          altText: parse(resp.texto),
          metaDescription: resp.metaDescription,
          caption: resp.titulo,
          link: `/post/${resp.slug}`,
          slug: resp.slug,
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
      .catch((error: any) => {
        console.log(error);
        setData({
          loaded: false,
          post: undefined,
        });
      });
  };

  React.useEffect(() => {
    if (settings) {
      const slug = props?.params?.slug ?? props?.match?.params?.slug ?? 1;
      fetchPosts(slug, settings.baseHref);
    }
  }, [props?.match?.params?.slug, props?.params?.slug, settings]);

  if (data.loaded && data.post) {
    return (
      <div className="App-home header-distance">
        <div className="container">
          <div
            className={`section padding-top section--large section--grey section--in-view article-list article-list--page-1`}
          >
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

            <article
              className={`article article--list article--blog article--1`}
            >
              <a
                href={`/post/${data.post.slug}`}
                className="article__image article__image--thumb"
              >
                <img src={data.post.imageSrc} alt={data.post.caption}></img>
              </a>
              <div className="article__inner">
                <h2 className="article__title">
                  <a href={`/post/${data.post.slug}`} rel="bookmark">
                    {data.post.title}
                  </a>
                </h2>

                <div dangerouslySetInnerHTML={{ __html: data.post.text }} />
                <div className="article__meta">
                  <p className="article__date">{data.post.date}</p>
                  <p className="article__author">{data.post.metaDescription}</p>
                </div>
              </div>
            </article>
          </div>
        </div>
      </div>
    );
  }

  return <div>Loading...</div>;
};

export default BlogPostComponent;
