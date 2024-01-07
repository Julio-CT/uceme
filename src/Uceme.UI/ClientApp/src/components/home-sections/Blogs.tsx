/* eslint-disable react/function-component-definition */
import * as React from 'react';
import parse from 'html-react-parser';
import { Container } from 'reactstrap';
import './Blogs.scss';
import { Link } from 'react-router-dom';
import { ReactElement } from 'react';
import tinyDate from '../../resources/images/tinydate.webp';
import photoIcon from '../../resources/images/photoicon.webp';
import BlogItem from '../../library/BlogItem';
import SettingsContext, { Settings } from '../../SettingsContext';
import BlogPostResponse from '../../library/BlogPostResponse';

type BlogState = {
  items: BlogItem[];
  isFetching: boolean;
};

const Blogs: () => ReactElement = () => {
  const settings: Settings = React.useContext(SettingsContext);
  const [data, setData] = React.useState<BlogState>({
    items: [] as Array<BlogItem>,
    isFetching: false,
  });

  React.useEffect(() => {
    async function fetchData() {
      if (settings?.baseHref !== undefined) {
        fetch(`${settings.baseHref}api/blog/getblogsubset?amount=3`, {
          headers: {},
        })
          .then((response: Response) => response.json())
          .then(async (posts: BlogPostResponse[]) => {
            const retrievedBlogs: BlogItem[] = [];

            await Promise.all(
              posts.map(async (obj: BlogPostResponse) => {
                const image = `${
                  process.env.PUBLIC_URL
                }/uploads/${obj.foto.slice(obj.foto.lastIndexOf('/') + 1)}`;
                retrievedBlogs.push({
                  id: obj.idBlog,
                  title: obj.titulo,
                  imageSrc: image,
                  text: obj.texto,
                  altText: parse(obj.texto),
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

            setData({ items: retrievedBlogs, isFetching: false });
          })
          .catch(() => {
            setData({ items: [] as Array<BlogItem>, isFetching: false });
          });
      }
    }

    fetchData();
  }, [settings.baseHref]);

  const posts = data.items.map((post) => {
    return (
      <div className="blogs col-12 col-md-4" key={post.id}>
        <div className="box-noticias" key={post.title}>
          <a href={post.link}>
            <img
              src={post.imageSrc}
              alt={post.title}
              className="img-post wp-post-image"
              width="370"
              height="230"
            />
          </a>
          <a href={post.link}>
            <img className="icono-img" src={photoIcon} alt="icon" />
          </a>
          <h5 className="uppercase post-title">
            <Link to={post.link}>{post.caption}</Link>
          </h5>
          <div className="line-small" />
          <p />
          <div className="post-ellipsis">{post.altText}</div>
          <p />
        </div>
        <div className="box-admin">
          <div className="date uppercase">
            <img src={tinyDate} alt="date" />
            {post.date}
          </div>
        </div>
      </div>
    );
  });

  if (settings) {
    return (
      <section id="section-blog" className="clearfix">
        <Container>
          {data.isFetching ? (
            <div>...Data Loading.....</div>
          ) : (
            <div className="blog container clearfix extra-margin">
              <h3 className="uppercase">Blog</h3>
              <h4 className="padding-y-medium spacing uppercase">
                Actualidad, agenda y noticias.
              </h4>
              <div className="line" />
              <div className="row justify-content-md-center">{posts}</div>
            </div>
          )}
        </Container>
      </section>
    );
  }

  return <section id="section-blog" className="clearfix" />;
};

export default Blogs;
