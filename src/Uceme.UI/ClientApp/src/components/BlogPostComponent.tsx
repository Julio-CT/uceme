import React from 'react';
import { Helmet } from 'react-helmet';
import BlogPost from '../library/BlogPost';

type BlogHomeState = {
  loaded: boolean;
  post?: BlogPost;
};

type BlogHomeProps = {
  children: React.ReactElement[];
  params?: any;
};

class BlogPostComponent extends React.Component<BlogHomeProps, BlogHomeState> {
  constructor(props: BlogHomeProps | Readonly<BlogHomeProps>) {
    super(props);

    this.state = {
      loaded: false,
    };
  }

  fetchPosts(slug: number): void {
    fetch(`api/blog/getblogs?slug=${slug}`)
      .then((response: { json: () => any }) => response.json())
      .then(async (resp: any) => {
        // const retrievedBlogs: blog[] = [];

        // await Promise.all(resp.map(async (obj: any) => {
        //     const image = await import('../../resources/images/' + obj.foto.slice(obj.foto.lastIndexOf('/') + 1));
        //     retrievedBlogs.push({
        //         id: obj.idBlog,
        //         title: obj.titulo,
        //         src: image.default,
        //         altText: parse(obj.texto),
        //         caption: obj.titulo,
        //         link: 'Cirugía de Glándulas Suprarrenales',
        //         date: new Intl.DateTimeFormat("en-GB", {
        //             year: "numeric",
        //             month: "long",
        //             day: "2-digit"
        //         }).format(new Date(obj.fecha)),
        //     });
        // }));

        this.setState({
          loaded: false,
          post: resp.data,
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
    const { slug } = this.props.params;

    this.fetchPosts(slug);
  }

  render() {
    const { post } = this.state;
    if (this.state.loaded && post) {
      return (
        <div>
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
          <div dangerouslySetInnerHTML={{ __html: post.imageSrc }} />
        </div>
      );
    }
    return <div>Loading...</div>;
  }
}

export default BlogPostComponent;
