import React from 'react';
import { Helmet } from 'react-helmet';
import BlogPost from '../library/BlogPost';
import parse from 'html-react-parser';

type BlogHomeState = {
  loaded: boolean;
  post?: BlogPost;
};

type BlogHomeProps = {
  children: React.ReactElement[];
  params?: any;
  history?: any;
  location?: any;
  match?: any;
};

class BlogPostComponent extends React.Component<BlogHomeProps, BlogHomeState> {
  constructor(props: BlogHomeProps | Readonly<BlogHomeProps>) {
    super(props);

    this.state = {
      loaded: false,
    };
  }

  fetchPosts(slug: string): void {
    fetch(`api/blog/getpost?slug=${slug}`)
      .then((response: { json: () => any }) => response.json())
      .then(async (resp: any) => {
        let retrievedBlog: BlogPost;
        const image = await import('../resources/images/' + resp.foto.slice(resp.foto.lastIndexOf('/') + 1));
        retrievedBlog = {
          id: resp.idBlog,
          title: resp.titulo,
          imageSrc: image.default,
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

        this.setState({
          loaded: true,
          post: retrievedBlog,
        });
      })
      .catch((error: any) => {
        console.log(error);
        this.setState({
          loaded: false,
          post: undefined,
        });
      });
  }

  UNSAFE_componentWillMount() {
    const slug = this.props?.params?.slug ?? this.props?.match?.params?.slug ?? 1;

    this.fetchPosts(slug);
  }

  render() {
    const { post } = this.state;
    if (this.state.loaded && post) {
      return (
        <div className="App App-home header-distance">
          <Helmet>
            <title>{post.seoTitle}</title>
            <meta name="description" content={post.metaDescription} />
            <meta name="og:image" content={post.featuredImage} />
            <meta property="og:url" content={post.link} />
            <meta property="og:type" content="article" />
            <meta property="og:title" content={post.title} />
            <meta property="og:description" content={post.metaDescription} />
          </Helmet>

          <h1>{post.title}</h1>
          <div dangerouslySetInnerHTML={{ __html: post.text }} />
        </div>
      );
    }

    return <div>Loading...</div>;
  }
}

export default BlogPostComponent;
