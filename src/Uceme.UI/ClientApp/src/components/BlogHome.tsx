import React from 'react';
import { Link } from 'react-router-dom';
import BlogPost from '../library/BlogPost';
import parse from 'html-react-parser';

type BlogHomeState = {
  loaded: boolean;
  resp?: any;
};

type BlogHomeProps = {
  children: React.ReactElement[];
  params?: any;
};

class BlogHome extends React.Component<BlogHomeProps, BlogHomeState> {
  constructor(props: BlogHomeProps) {
    super(props);

    this.state = {
      loaded: false,
      resp: null,
    };
  }

  fetchPosts(page: number): void {
    fetch(`api/blog/getbloglist?page=${page}`)
      .then((response: { json: () => any }) => response.json())
      .then(async (resp: any) => {
        const retrievedBlogs: BlogPost[] = [];

        await Promise.all(resp.map(async (obj: any) => {
            const image = await import('../resources/images/' + obj.foto.slice(obj.foto.lastIndexOf('/') + 1));
            retrievedBlogs.push({
                id: obj.idBlog,
                title: obj.titulo,
                imageSrc: image.default,
                altText: parse(obj.texto),
                caption: obj.titulo,
                link: 'Cirugía de Glándulas Suprarrenales',
                date: new Intl.DateTimeFormat("en-GB", {
                    year: "numeric",
                    month: "long",
                    day: "2-digit"
                }).format(new Date(obj.fecha)),
            });
        }));

        this.setState({
          loaded: false,
          resp: resp.data,
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

  UNSAFE_componentWillMount() {
    const page = this.props?.params?.page || 1;

    this.fetchPosts(page);
  }

  UNSAFE_componentWillReceiveProps(nextProps: BlogHomeProps) {
    this.setState({ loaded: false });

    const page = nextProps.params?.page || 1;

    this.fetchPosts(page);
  }

  render() {
    if (this.state.loaded) {
      const { next_page, previous_page } = this.state.resp.meta;

      return (
        <div>
          {this.state.resp.data.map((post: BlogPost) => {
            return (
              <div key={post.slug}>
                <Link to={`/post/${post.slug}`}>{post.title}</Link>
              </div>
            );
          })}

          <br />

          <div>
            {previous_page && <Link to={`/p/${previous_page}`}>Prev</Link>}

            {next_page && <Link to={`/p/${next_page}`}>Next</Link>}
          </div>
        </div>
      );
    }

    return <div>Loading...</div>;
  }
}

export default BlogHome;
