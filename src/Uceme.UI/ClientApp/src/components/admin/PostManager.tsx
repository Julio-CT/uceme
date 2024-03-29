import React, { ReactElement, useRef } from 'react';
import { useParams } from 'react-router';
import { Modal, ModalBody, ModalFooter, Button } from 'reactstrap';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import parse from 'html-react-parser';
import BlogItem from '../../library/BlogItem';
import './AppointmentManager.scss';
import SettingsContext, { Settings } from '../../SettingsContext';
import authService from '../api-authorization/AuthorizeService';
import AddPostModal from './AddPostModal';
import BlogPostResponse from '../../library/BlogPostResponse';

type PostManagerState = {
  loaded: boolean;
  posts?: BlogItem[] | null;
  page?: number;
};

function PostManager(): ReactElement {
  const { page } = useParams();
  const [addModal, setAddModal] = React.useState(false);
  const addToggle = () => setAddModal(!addModal);
  const [editModal, setEditModal] = React.useState(false);
  const editToggle = () => setEditModal(!editModal);
  const [confirmModal, setConfirmModal] = React.useState(false);
  const confirmToggle = () => setConfirmModal(!confirmModal);
  const [alertModal, setAlertModal] = React.useState<boolean>(false);
  const alertToggle = () => setAlertModal(!alertModal);
  const [alertMessage, setAlertMessage] = React.useState<string>('');
  const [markedPost, setMarkedPost] = React.useState<BlogItem>();
  const settings: Settings = React.useContext(SettingsContext);
  const [postData, setPostData] = React.useState<PostManagerState>({
    loaded: false,
    posts: null,
    page: page ? +page : 1,
  });

  const isFirstRun = useRef(true);

  const fetchPosts = async (pageToFetch: number, baseHref: string) => {
    const token = await authService.getAccessToken();
    fetch(`${baseHref}api/blog/getallposts`, {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    })
      .then((response: { json: () => Promise<BlogPostResponse[]> }) =>
        response.json()
      )
      .then(async (resp: BlogPostResponse[]) => {
        const retrievedPosts: BlogItem[] = [];

        await Promise.all(
          resp.map(async (obj: BlogPostResponse) => {
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
              seoTitle: obj.seoTitle,
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
          page: pageToFetch,
        });
      })
      .catch(() => {
        setPostData({
          loaded: false,
          posts: null,
          page: pageToFetch,
        });
      });
  };

  const editPost = (post: BlogItem) => {
    setMarkedPost(post);
    setEditModal(true);
  };

  const deletePost = (post: BlogItem) => {
    setMarkedPost(post);
    setConfirmModal(true);
  };

  const deleteMarkedPost = async () => {
    if (settings?.baseHref !== undefined && markedPost) {
      setConfirmModal(false);
      const token = await authService.getAccessToken();
      fetch(
        `${settings?.baseHref}api/blog/deletepost?postid=${+markedPost.id}`,
        {
          headers: !token ? {} : { Authorization: `Bearer ${token}` },
        }
      )
        .then((response: { json: () => Promise<boolean> }) => response.json())
        .then(async (resp: boolean) => {
          if (resp === true) {
            setAlertMessage('Post borrado correctamente. Muchas gracias.');
            alertToggle();
            setPostData({
              loaded: true,
              posts: postData.posts?.filter(
                (obj: BlogItem) => obj.id !== markedPost.id
              ),
              page: postData.page,
            });
          } else {
            setAlertMessage(
              'Lo sentimos, ha ocurrido un error borrando el post. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
            );
          }
        })
        .catch(() => {
          setAlertMessage(
            'Lo sentimos, ha ocurrido un error borrando el post. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
          );
        });
    }
  };

  React.useEffect(() => {
    if (settings?.baseHref !== undefined) {
      const currentPage = page ? +page : 1;

      if (isFirstRun.current) {
        isFirstRun.current = false;

        fetchPosts(currentPage, settings.baseHref);
        return;
      }

      setPostData({ loaded: false, page: currentPage });
      fetchPosts(currentPage, settings.baseHref);
    }
  }, [page, settings?.baseHref]);

  if (postData.loaded) {
    return (
      <div className="app app-home header-distance-l">
        <p>
          <br />
          <Button
            color="primary"
            onClick={addToggle}
            onKeyDown={addToggle}
            tabIndex={0}
          >
            Añadir Post
          </Button>
        </p>
        <Modal isOpen={alertModal} toggle={alertToggle}>
          <ModalBody>
            <section id="section-contact_form" className="container">
              <div className="row justify-content-md-center">
                {alertMessage}
              </div>
            </section>
          </ModalBody>
          <ModalFooter>
            <Button color="secondary" onClick={alertToggle}>
              Cerrar
            </Button>
          </ModalFooter>
        </Modal>
        <AddPostModal
          key="editModal"
          modal={editModal}
          toggle={editToggle}
          post={markedPost}
          headerTitle="Editar Post"
        />
        <AddPostModal
          key="addModal"
          modal={addModal}
          toggle={addToggle}
          headerTitle="Nuevo Post"
        />
        <Modal key="confirmModal" isOpen={confirmModal} toggle={confirmToggle}>
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
            </Button>{' '}
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
                <th scope="col">Editar</th>
                <th scope="col">Borrar</th>
              </tr>
            </thead>
            <tbody>
              {postData.posts?.map((post: BlogItem) => {
                return (
                  <tr key={post.id}>
                    <td>{post.date}</td>
                    <td>{post.title}</td>
                    <td>
                      <EditIcon
                        aria-label="Edit"
                        className="clickable"
                        onClick={() => editPost(post)}
                      />
                    </td>
                    <td>
                      <DeleteIcon
                        aria-label="Delete"
                        className="clickable"
                        onClick={() => deletePost(post)}
                      />
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

  return <div className="app app-home header-distance">Loading...</div>;
}

export default PostManager;
