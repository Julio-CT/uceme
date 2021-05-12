import React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import parse from 'html-react-parser';
import BlogPost from '../library/BlogPost';
import './BlogHome.scss';
import SettingsContext from '../SettingsContext';
import BlogPostResponse from '../library/BlogPostResponse';

type BlogHomeState = {
  loaded: boolean;
  resp?: BlogPost[] | null;
  page?: number;
};

interface MatchParams {
  page: string;
}

type BlogHomeProps = RouteComponentProps<MatchParams>;

const BlogHome = (props: BlogHomeProps): JSX.Element => {
  const settings = React.useContext(SettingsContext());
  const { match } = props;
  const [data, setData] = React.useState<BlogHomeState>({
    loaded: false,
    resp: null,
    page: +match?.params?.page ?? 1,
  });

  const isFirstRun = React.useRef(true);

  const fetchPosts = (page: number, baseHref: string) => {
    fetch(`${baseHref}api/blog/getbloglist?page=${page}`)
      .then((response: Response) => response.json())
      .then(async (resp: BlogPostResponse[]) => {
        const retrievedBlogs: BlogPost[] = [];

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
          page,
        });
      })
      .catch(() => {
        setData({
          loaded: false,
          resp: null,
          page,
        });
      });
  };

  React.useEffect(() => {
    if (settings) {
      const page = +match?.params?.page || 1;

      if (isFirstRun.current) {
        isFirstRun.current = false;

        fetchPosts(page, settings.baseHref);
        return;
      }

      setData({ loaded: false, page });
      fetchPosts(page, settings.baseHref);
    }
  }, [match?.params?.page, settings]);

  if (data.loaded) {
    const nextPage: number = data.page ? +data.page + 1 : 2;
    const previousPage: number | undefined =
      data.page && +data.page !== 1 ? +data.page - 1 : undefined;

    return (
      <div className="App App-home header-distance">
        <div className="container">
          <div
            className={`section padding-top section--large section--grey section--in-view article-list article-list--page-${data.page}`}
          >
            {data.resp?.map((post: BlogPost, index: number) => {
              return (
                <React.Fragment key={post.slug}>
                  <article
                    className={`article article--list article--blog article--${
                      +index + 1
                    }`}
                  >
                    <a
                      href={`/post/${post.slug}`}
                      className="article__image article__image--thumb"
                    >
                      <img src={post.imageSrc} alt={post.caption} />
                    </a>
                    <div className="article__inner">
                      <h2 className="article__title">
                        <a href={`/post/${post.slug}`} rel="bookmark">
                          {post.title}
                        </a>
                      </h2>

                      <div className="article__meta">
                        <p className="article__date">{post.date}</p>
                        <p className="article__author">
                          {post.metaDescription}
                        </p>
                      </div>
                    </div>
                  </article>
                </React.Fragment>
              );
            })}
            <br />
            <div>
              {previousPage && (
                <Link to={`/blog/${previousPage}`}>{'<<'} Anterior</Link>
              )}
              {' | '}
              {nextPage && (
                <Link to={`/blog/${nextPage}`}>Siguiente {'>>'} </Link>
              )}
            </div>
          </div>
        </div>
      </div>
    );
  }

  return <div className="App App-home header-distance">Loading...</div>;
};

export default BlogHome;
