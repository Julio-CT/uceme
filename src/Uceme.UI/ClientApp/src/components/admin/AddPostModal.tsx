import * as React from 'react';
import {
  Button,
  Input,
  Label,
  Modal,
  ModalBody,
  ModalFooter,
  ModalHeader,
} from 'reactstrap';
import DatePicker from 'reactstrap-date-picker2';
import { ContentState, convertToRaw, RawDraftContentState } from 'draft-js';
import { Editor } from 'react-draft-wysiwyg';
import draftToHtml from 'draftjs-to-html';
import SettingsContext from '../../SettingsContext';
import authService from '../api-authorization/AuthorizeService';
import './AddPostModal.scss';
import 'react-draft-wysiwyg/dist/react-draft-wysiwyg.css';
import BlogPost from '../../library/BlogPost';

type AddPostModalProps = {
  toggle: () => void;
  modal?: boolean;
  post?: BlogPost;
  headerTitle: string;
};

const AddPostModal = (props: AddPostModalProps): JSX.Element => {
  const { modal, toggle, post, headerTitle } = props;

  let contentState = ContentState.createFromText(post ? post.text : '');
  let raw = convertToRaw(contentState);

  const settings = React.useContext(SettingsContext());
  const inputName = 'reactstrap_date_picker_basic';
  const [currentPost, setCurrentPost] =
    React.useState<BlogPost | undefined>(post);
  const [photo, setPhoto] = React.useState<string | Blob>(
    post ? post.imageSrc : ''
  );
  const [selectedDay, setDay] = React.useState<string>(
    post ? post.date : `${new Date().toISOString().slice(0, 10)}T00:00:00.000Z`
  );
  const [title, setTitle] = React.useState<string>(
    post && post.title ? post.title : ''
  );
  const [slug, setSlug] = React.useState<string>(
    post && post.slug ? post.slug : ''
  );
  const [text, setText] = React.useState<RawDraftContentState>(raw);
  const [caption, setCaption] = React.useState<string>(
    post && post.caption ? post.caption : ''
  );
  const [metaDescription, setMetaDescription] = React.useState<string>(
    post && post.metaDescription ? post.metaDescription : ''
  );
  const [seoTitle, setSeoTitle] = React.useState<string>(
    post && post.seoTitle ? post.seoTitle : ''
  );
  const [imgSrc, setImgSrc] = React.useState<string>(
    post && post.imageSrc ? post.imageSrc : ''
  );
  const [id, setId] = React.useState<number>(post && post.id ? +post.id : 0);

  const weekStart = 1;

  const resetForm = () => {
    setTitle('');
    setSlug('');
    setText(raw);
    setCaption('');
  };

  const handleValidation = () => {
    const errors: Record<string, string> = {};
    let formIsValid = true;

    if (!selectedDay) {
      formIsValid = false;
      errors.day = 'Cannot be empty';
    }

    if (!title) {
      formIsValid = false;
      errors.title = 'Cannot be empty';
    }

    if (!slug) {
      formIsValid = false;
      errors.slug = 'Cannot be empty';
    }

    if (!text) {
      formIsValid = false;
      errors.text = 'Cannot be empty';
    }

    if (!caption) {
      formIsValid = false;
      errors.caption = 'Cannot be empty';
    }

    return formIsValid;
  };

  const setFile = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target?.files) {
      setPhoto(e.target?.files[0]);
    }
  };

  const uploadfile = async (
    evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    evt.preventDefault();
    const formData = new FormData();
    formData.append('file', photo);

    const token = await authService.getAccessToken();

    try {
      fetch(`clientapi/blog/onpostuploadasync`, {
        method: 'POST',
        mode: 'cors',
        body: formData,
        headers: !token
          ? { Accept: 'application/json' }
          : { Accept: 'application/json', Authorization: `Bearer ${token}` },
      })
        .then((response: { json: () => Promise<string> }) => response.json())
        .then(async (resp: string) => {
          if (resp) {
            setImgSrc(resp);
            alert(`Imagen subida correctamente.`);
          } else {
            alert(
              'Lo sentimos, ha ocurrido un error subiendo la imagen. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros.'
            );
          }
        })
        .catch(() => {
          alert(
            'Lo sentimos, ha ocurrido un error subiendo la imagen. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros.'
          );
        });
    } catch (error) {
      console.error('Error:', error);
    }
  };

  const submitForm = (evt: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    evt.preventDefault();
    if (handleValidation() && settings) {
      const day = new Date(selectedDay);
      const data = {
        idBlog: id,
        titulo: title,
        slug,
        texto: draftToHtml(text),
        caption,
        fecha: day,
        seoTitle,
        metadescription: metaDescription,
        foto: imgSrc,
      };

      fetch(`clientapi/blog/addpost`, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
          'Content-Type': 'application/json',
          // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data), // body data type must match "Content-Type" header
      })
        .then((response: { json: () => Promise<boolean> }) => response.json())
        .then(async (resp: boolean) => {
          if (resp === true) {
            alert('Post registrado correctamente. Muchas gracias.');
            resetForm();
            props.toggle();
          } else {
            alert(
              'Lo sentimos, ha ocurrido un error registrando su post. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
            );
          }
        })
        .catch(() => {
          alert(
            'Lo sentimos, ha ocurrido un error registrando su post. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros.'
          );
        });
    }
  };

  if (post !== currentPost) {
    setCurrentPost(post);
    contentState = ContentState.createFromText(post ? post.text : '');
    raw = convertToRaw(contentState);
    setPhoto(post ? post.imageSrc : '');
    // setDay(post ? post.date : '');
    const t = post && post.title ? post.title : '';
    setTitle(t);
    setSlug(post && post.slug ? post.slug : '');
    setText(raw);
    setCaption(post ? post.caption : '');
    setMetaDescription(
      post && post.metaDescription ? post.metaDescription : ''
    );
    setSeoTitle(post && post.seoTitle ? post.seoTitle : '');
    setImgSrc(post ? post.imageSrc : '');
    setId(post ? +post.id : 0);
  }

  return (
    <Modal isOpen={modal} toggle={toggle}>
      <ModalHeader toggle={toggle} className="beatabg">
        <div className="Aligner">
          <div className="Aligner-item Aligner-item--top" />
          <div className="Aligner-item">{headerTitle}</div>
          <div className="Aligner-item Aligner-item--bottom" />
        </div>
      </ModalHeader>
      <ModalBody>
        <section id="section-contact_form" className="container">
          <div className="row justify-content-md-center">
            <form className="col-12">
              <div className="extra-padding field-margin">
                <Label for="FileUpload_FormFile">Foto</Label>
                <Input
                  id="FileUpload_FormFile"
                  type="file"
                  name="FileUpload.FormFile"
                  onChange={(e) => setFile(e)}
                />
                <Button
                  className="submit-form-button"
                  onClick={(e) => uploadfile(e)}
                  value="Subir"
                >
                  Subir foto
                </Button>
              </div>
            </form>
            <form className="col-12">
              <div className="extra-padding field-margin">
                <Label for="titleForm" className="field-label">
                  Título
                </Label>
                <Input
                  type="text"
                  name="titleForm"
                  id="titleForm"
                  placeholder="Campo requerido"
                  value={title}
                  onChange={(evt) => setTitle(evt.target.value)}
                  required
                />
              </div>
              <div className="field-margin">
                <Label for="dateForm" className="field-label">
                  Fecha de publicación
                </Label>
                <DatePicker
                  id="dateForm"
                  name={inputName}
                  value={selectedDay}
                  onChange={(v: React.SetStateAction<string>) => {
                    setDay(v);
                  }}
                  weekStartsOn={weekStart}
                  minDate={`${new Date()
                    .toISOString()
                    .slice(0, 10)}T00:00:00.000Z`}
                />
                <Label for="slugForm" className="field-label">
                  Slug (link)
                </Label>
                <Input
                  type="text"
                  name="slugForm"
                  id="slugForm"
                  placeholder="Campo requerido"
                  value={slug}
                  onChange={(evt) => setSlug(evt.target.value)}
                  required
                />
                <Label for="textForm" className="field-label">
                  Texto
                </Label>
                <Editor
                  defaultContentState={text}
                  onContentStateChange={setText}
                  wrapperClassName="wrapper-class"
                  editorClassName="editor-class"
                  toolbarClassName="toolbar-class"
                />
                <Label for="captionForm" className="field-label">
                  Caption (descripción de la imagen)
                </Label>
                <Input
                  type="textarea"
                  name="captionForm"
                  id="captionForm"
                  value={caption}
                  onChange={(evt) => setCaption(evt.target.value)}
                />
                <Label for="metaForm" className="field-label">
                  Meta - description
                </Label>
                <Input
                  type="textarea"
                  name="metaForm"
                  id="metaForm"
                  value={metaDescription}
                  onChange={(evt) => setMetaDescription(evt.target.value)}
                />
                <Label for="seoForm" className="field-label">
                  Titulo para SEO
                </Label>
                <Input
                  type="textarea"
                  name="seoForm"
                  id="seoForm"
                  value={seoTitle}
                  onChange={(evt) => setSeoTitle(evt.target.value)}
                />
              </div>
            </form>
          </div>
        </section>
      </ModalBody>
      <ModalFooter>
        <Button className="submit-form-button" onClick={(e) => submitForm(e)}>
          Publicar
        </Button>{' '}
        <Button color="secondary" onClick={toggle}>
          Cancelar
        </Button>
      </ModalFooter>
    </Modal>
  );
};

AddPostModal.defaultProps = {
  modal: false,
  post: null,
};

export default AddPostModal;
