import React from 'react';
import { Helmet } from 'react-helmet';
import BlogPost from '../library/BlogPost';
import parse from 'html-react-parser';
import SettingsContext from '../SettingsContext';

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
      <div className="App App-home header-distance">
        <Helmet>
          <title>{data.post.seoTitle}</title>
          <meta name="description" content={data.post.metaDescription} />
          <meta name="og:image" content={data.post.featuredImage} />
          <meta property="og:url" content={data.post.link} />
          <meta property="og:type" content="article" />
          <meta property="og:title" content={data.post.title} />
          <meta property="og:description" content={data.post.metaDescription} />
        </Helmet>

        <h1>{data.post.title}</h1>
        <div dangerouslySetInnerHTML={{ __html: data.post.text }} />
      </div>
    );
  }

  return <div>Loading...</div>;
};

export default BlogPostComponent;
