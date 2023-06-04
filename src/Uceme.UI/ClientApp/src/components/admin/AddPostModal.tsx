/* eslint-disable no-alert */
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
import htmlToDraft from 'html-to-draftjs';
import authService from '../api-authorization/AuthorizeService';
import './AddPostModal.scss';
import 'react-draft-wysiwyg/dist/react-draft-wysiwyg.css';
import BlogItem from '../../library/BlogItem';
import SettingsContext, {Settings} from '../../SettingsContext';

type AddPostModalProps = {
  toggle: () => void;
  modal?: boolean;
  post?: BlogItem;
  headerTitle: string;
};

function AddPostModal(props: AddPostModalProps): JSX.Element {
  const { modal, toggle, post, headerTitle } = props;
  const settings: Settings = React.useContext(SettingsContext);

  let contentState = ContentState.createFromText(post ? post.text : '');
  const inputName = 'reactstrap_date_picker_basic';
  const [currentPost, setCurrentPost] = React.useState<BlogItem | undefined>(
    post
  );
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
  const [text, setText] = React.useState<RawDraftContentState>(
    convertToRaw(contentState)
  );
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
    setText(convertToRaw(contentState));
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

    fetch(`${settings?.baseHref}api/blog/onpostuploadasync`, {
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
      });
  };

  const submitForm = async (
    evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    evt.preventDefault();
    if (handleValidation()) {
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

      const token = await authService.getAccessToken();
      fetch(`${settings?.baseHref}api/blog/addpost`, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: !token
          ? {
              // 'Content-Type': 'application/x-www-form-urlencoded',
            }
          : {
              'Content-Type': 'application/json',
              Authorization: `Bearer ${token}`,
            },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data), // body data type must match "Content-Type" header
      })
        .then((response: { json: () => Promise<boolean> }) => response.json())
        .then(async (resp: boolean) => {
          if (resp) {
            alert('Post registrado correctamente. Muchas gracias.');
            resetForm();
            toggle();
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
    contentState = ContentState.createFromBlockArray(
      post ? htmlToDraft(post.text).contentBlocks : []
    );
    setPhoto(post ? post.imageSrc : '');
    // setDay(post ? post.date : '');
    const t = post && post.title ? post.title : '';
    setTitle(t);
    setSlug(post && post.slug ? post.slug : '');
    setText(convertToRaw(contentState));
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
        <div className="aligner">
          <div className="aligner-item aligner-item-top" />
          <div className="aligner-item">{headerTitle}</div>
          <div className="aligner-item aligner-item-bottom" />
        </div>
      </ModalHeader>
      <ModalBody>
        <section id="section-contact_form" className="container">
          <div className="row justify-content-md-center">
            <form className="col-12">
              <div className="field-margin">
                <Label for="FileUpload_FormFile" className="field-label">
                  Imagen (max 600x600px):
                </Label>
                <Input
                  id="FileUpload_FormFile"
                  type="file"
                  name="FileUpload_FormFile"
                  accept=".jpg, .jpeg, .png, .webp"
                  onChange={(e) => setFile(e)}
                />
                <Button
                  className="submit-form-button top-margin"
                  onClick={(e) => uploadfile(e)}
                  value="Subir"
                  disabled={!photo || !!imgSrc}
                >
                  Subir imagen
                </Button>
              </div>
            </form>
            <form className="col-12">
              <div className="field-margin">
                <Label for="titleForm" className="field-label">
                  Título:
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
                  Fecha de publicación:
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
                  Slug (link):
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
                  Texto:
                </Label>
                <Editor
                  defaultContentState={text}
                  onContentStateChange={setText}
                  wrapperClassName="wrapper-class"
                  editorClassName="editor-class"
                  toolbarClassName="toolbar-class"
                />
                <Label for="captionForm" className="field-label">
                  Caption (descripción de la imagen):
                </Label>
                <Input
                  type="textarea"
                  name="captionForm"
                  id="captionForm"
                  value={caption}
                  onChange={(evt) => setCaption(evt.target.value)}
                />
                <Label for="metaForm" className="field-label">
                  Meta-description:
                </Label>
                <Input
                  type="textarea"
                  name="metaForm"
                  id="metaForm"
                  value={metaDescription}
                  onChange={(evt) => setMetaDescription(evt.target.value)}
                />
                <Label for="seoForm" className="field-label">
                  Título para SEO:
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
        <Button
          className="submit-form-button"
          onClick={(e) => submitForm(e)}
          disabled={
            !imgSrc ||
            !title ||
            !caption ||
            !text ||
            !metaDescription ||
            !selectedDay ||
            !seoTitle
          }
        >
          Publicar
        </Button>{' '}
        <Button color="secondary" onClick={toggle}>
          Cancelar
        </Button>
      </ModalFooter>
    </Modal>
  );
}

AddPostModal.defaultProps = {
  modal: false,
  post: null,
};

export default AddPostModal;
