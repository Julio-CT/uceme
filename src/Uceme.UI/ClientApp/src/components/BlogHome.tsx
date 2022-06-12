import React from 'react';
import { Link, useParams } from 'react-router-dom';
import parse from 'html-react-parser';
import BlogItem from '../library/BlogItem';
import './BlogHome.scss';
import SettingsContext from '../SettingsContext';
import BlogPostResponse from '../library/BlogPostResponse';

type BlogHomeState = {
  loaded: boolean;
  resp?: BlogItem[] | null;
  page?: number;
};

interface MatchParams {
  page: string;
}

function BlogHome(): JSX.Element {
  const settings = React.useContext(SettingsContext);
  const { page } = useParams<MatchParams>();
  const [data, setData] = React.useState<BlogHomeState>({
    loaded: false,
    resp: null,
    page: +page ?? 1,
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
            page: +page,
          });
        })
        .catch(() => {
          setData({
            loaded: false,
            resp: null,
            page: +page,
          });
        });
    };

    if (settings) {
      const pageNumber = +page || 1;

      if (isFirstRun.current) {
        isFirstRun.current = false;

        fetchPosts(pageNumber, settings.baseHref);
        return;
      }

      setData({ loaded: false, page: pageNumber });
      fetchPosts(pageNumber, settings.baseHref);
    }
  }, [page, settings]);

  if (data.loaded) {
    const nextPage: number = data.page ? +data.page + 1 : 2;
    const previousPage: number | undefined =
      data.page && +data.page !== 1 ? +data.page - 1 : undefined;

    return (
      <div className="app app-home header-distance">
        <div className="container" data-testid="blogContainer">
          <div
            className={`section padding-top section-large section-grey section-in-view article-list-container article-list-page-${data.page}`}
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

  return <div className="app app-home header-distance">Loading...</div>;
}

export default BlogHome;
