import React, { useRef } from 'react';
import { Modal, ModalBody, ModalFooter, Button } from 'reactstrap';
import DeleteIcon from '@material-ui/icons/Delete';
import parse from 'html-react-parser';
import BlogPost from '../../library/BlogPost';
import './AppointmentManager.scss';
import SettingsContext from '../../SettingsContext';
import authService from '../api-authorization/AuthorizeService';
import AddPostModal from './AddPostModal'

type PostManagerState = {
  loaded: boolean;
  posts?: any;
  page?: number;
};

type PostManagerProps = {
  params?: any;
  match?: any;
};

const PostManager = (props: PostManagerProps): JSX.Element => {
  const [modal, setModal] = React.useState(false);
  const toggle = () => setModal(!modal);
  const [confirmModal, setConfirmModal] = React.useState(false);
  const confirmToggle = () => setConfirmModal(!confirmModal);
  const [markedPost, setMarkedPost] = React.useState<BlogPost>();
  const settings = React.useContext(SettingsContext());
  const [postData, setPostData] = React.useState<PostManagerState>({
    loaded: false,
    posts: null,
    page: props?.params?.page ?? props?.match?.params?.page ?? 1,
  });

  const isFirstRun = useRef(true);

  const fetchPosts = async (page: number, baseHref: string) => {
    fetch(`${baseHref}api/blog/getallposts`)
      .then((response: { json: () => any }) => response.json())
      .then(async (resp: any) => {
        const retrievedPosts: BlogPost[] = [];

        await Promise.all(
          resp.map(async (obj: any) => {
            const image = `${process.env.PUBLIC_URL}/uploads/${obj.foto.slice(
              obj.foto.lastIndexOf('/') + 1
            )}`;
            retrievedPosts.push({
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

        setPostData({
          loaded: true,
          posts: retrievedPosts,
          page,
        });
      })
      .catch((error: any) => {
        console.log(error);
        setPostData({
          loaded: false,
          posts: null,
          page,
        });
      });
  };

  const deletePost = (post: BlogPost) => {
    setMarkedPost(post);
    setConfirmModal(true);
  };

  const deleteMarkedPost = async () => {
    if (settings && markedPost) {
      setConfirmModal(false);
      const token = await authService.getAccessToken();
      fetch(`clientapi/blog/deletepost?postid=${+markedPost.id}`, {
        headers: !token ? {} : { Authorization: `Bearer ${token}` },
      })
        .then((response: { json: () => any }) => response.json())
        .then(async (resp: any) => {
          if (resp === true) {
            alert('Post borrado correctamente. Muchas gracias.');
            setPostData({
              loaded: true,
              posts: postData.posts.filter((obj: BlogPost) => obj.id !== markedPost.id),
              page: postData.page,
            });
          } else {
            alert(
              'Lo sentimos, ha ocurrido un error borrando su post. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
            );
          }
        })
        .catch((error: any) => {
          console.log(error);
          alert(
            'Lo sentimos, ha ocurrido un error borrando su post. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
          );
        });
    }
  };

  React.useEffect(() => {
    if (settings) {
      const page = props?.params?.page || props?.match?.params?.page || 1;

      if (isFirstRun.current) {
        isFirstRun.current = false;

        fetchPosts(page, settings.baseHref);
        return;
      }

      setPostData({ loaded: false, page });
      fetchPosts(page, settings.baseHref);
    }
  }, [props?.match?.params?.page, props?.params?.page, settings]);

  if (postData.loaded) {
    return (
        <div className="App App-home header-distance">
        <p>
            <br />
            <Button color="primary" onClick={toggle}
                onKeyDown={toggle} tabIndex={0}>
                    Añadir Post
            </Button>
        </p>

        <AddPostModal modal={modal} toggle={toggle} />
        <Modal isOpen={confirmModal} toggle={confirmToggle}>
          <ModalBody>
            <section id="section-contact_form" className="container">
              <div className="row justify-content-md-center">
                ¿Está seguro de borrar el post {markedPost?.title}?
              </div>
            </section>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" onClick={() => deleteMarkedPost()}>
              Borrar
            </Button>
            {' '}
            <Button color="secondary" onClick={confirmToggle}>
              Cerrar
            </Button>
          </ModalFooter>
        </Modal>
        <div className="container">
          <table className="table">
            <thead>
              <tr>
                <th scope="col">Fecha</th>
                <th scope="col">Título</th>
                <th scope="col">Borrar</th>
              </tr>
            </thead>
            <tbody>
              {postData.posts.map((post: BlogPost, index: number) => {
                return (
                  <tr key={post.id}>
                    <td className="col-md-2">{post.date}</td>
                    <td className="col-md-3">{post.title}</td>
                    <td className="col-md-1">
                      <DeleteIcon onClick={() => deletePost(post)} />
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </div>
    );
  }

  return <div className="App App-home header-distance">Loading...</div>;
};

export default PostManager;
