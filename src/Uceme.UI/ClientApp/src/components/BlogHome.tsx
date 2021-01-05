import React, { Fragment } from 'react';
import { Link } from 'react-router-dom';
import parse from 'html-react-parser';
import BlogPost from '../library/BlogPost';
import './BlogHome.scss';

type BlogHomeState = {
  loaded: boolean;
  resp?: any;
  page?: number;
};

type BlogHomeProps = {
  children: React.ReactElement[];
  params?: any;
  history?: any;
  location?: any;
  match?: any;
};

class BlogHome extends React.Component<BlogHomeProps, BlogHomeState> {
  constructor(props: BlogHomeProps) {
    super(props);

    this.state = {
      loaded: false,
      resp: null,
      page: this.props?.params?.page ?? this.props?.match?.params?.page ?? 1,
    };
  }

  UNSAFE_componentWillMount() {
    const page =
      this.props?.params?.page || this.props?.match?.params?.page || 1;

    this.setState({ page: page });
    debugger;
    this.fetchPosts(page);
  }

  UNSAFE_componentWillReceiveProps(nextProps: BlogHomeProps) {
    const page = nextProps.params?.page || nextProps.match?.params?.page || 1;

    this.setState({ loaded: false, page: page });
    this.fetchPosts(page);
  }

  fetchPosts(page: number): void {
    fetch(`api/blog/getbloglist?page=${page}`)
      .then((response: { json: () => any }) => response.json())
      .then(async (resp: any) => {
        const retrievedBlogs: BlogPost[] = [];

        await Promise.all(
          resp.map(async (obj: any) => {
            const image = await import(
              `../resources/images/${obj.foto.slice(
                obj.foto.lastIndexOf('/') + 1
              )}`
            );
            retrievedBlogs.push({
              id: obj.idBlog,
              title: obj.titulo,
              imageSrc: image.default,
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

        this.setState({
          loaded: true,
          resp: retrievedBlogs,
        });
      })
      .catch((error: any) => {
        console.log(error);
        this.setState({
          loaded: false,
          resp: null,
        });
      });
  }

  render() {
    if (this.state.loaded) {
      const nextPage: number = this.state.page ? +this.state.page + 1 : 2;
      const previousPage: number | undefined =
        this.state.page && +this.state.page !== 1
          ? +this.state.page - 1
          : undefined;

      return (
        <div className="App App-home header-distance">
          <div className="container">
              <div
                className={`section padding-top section--large section--grey section--in-view article-list article-list--page-${this.state.page}`}
              >
                {this.state.resp.map((post: BlogPost, index: number) => {
                  return (
                    <Fragment key={post.slug}>
                      <article
                        className={`article article--list article--blog article--${
                          +index + 1
                        }`}
                      >
                        <a
                          href={`/post/${post.slug}`}
                          className="article__image article__image--thumb"
                        >
                          <img src={post.imageSrc} alt={post.caption}></img>
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
                    </Fragment>
                  );
                })}
                <br />
                <div>
                  {previousPage && (
                    <Link to={`/blog/${previousPage}`}>Anterior</Link>
                  )}

                  {nextPage && <Link to={`/blog/${nextPage}`}>Siguiente</Link>}
                </div>
              </div>
          </div>
        </div>
      );
    }

    return <div className="App App-home header-distance">Loading...</div>;
  }
}

export default BlogHome;
