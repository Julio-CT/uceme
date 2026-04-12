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
import { ReactElement } from 'react';
import authService from '../api-authorization/AuthorizeService';
import './AddPostModal.scss';
import 'react-draft-wysiwyg/dist/react-draft-wysiwyg.css';
import BlogItem from '../../library/BlogItem';
import SettingsContext, { Settings } from '../../SettingsContext';

type AddPostModalProps = {
  toggle: () => void;
  modal?: boolean;
  post?: BlogItem;
  headerTitle: string;
};

const emptyEditorState = (): RawDraftContentState =>
  convertToRaw(ContentState.createFromText(''));

function AddPostModal(props: AddPostModalProps): ReactElement {
  const { modal, toggle, post, headerTitle } = props;
  const settings: Settings = React.useContext(SettingsContext);

  const [alertModal, setAlertModal] = React.useState<boolean>(false);
  const alertToggle = () => setAlertModal(!alertModal);
  const [alertMessage, setAlertMessage] = React.useState<string>('');

  const inputName = 'reactstrap_date_picker_basic';
  const [newPostSession, setNewPostSession] = React.useState(0);
  const [photo, setPhoto] = React.useState<string | Blob>('');
  const [selectedDay, setDay] = React.useState<string>(
    `${new Date().toISOString().slice(0, 10)}T00:00:00.000Z`
  );
  const [title, setTitle] = React.useState<string>('');
  const [slug, setSlug] = React.useState<string>('');
  const [text, setText] =
    React.useState<RawDraftContentState>(emptyEditorState);
  const [caption, setCaption] = React.useState<string>('');
  const [metaDescription, setMetaDescription] = React.useState<string>('');
  const [seoTitle, setSeoTitle] = React.useState<string>('');
  const [imgSrc, setImgSrc] = React.useState<string>('');
  const [id, setId] = React.useState<number>(0);
  const [uploadSuccess, setUploadSuccess] = React.useState<boolean>(false);

  const weekStart = 1;

  const resetNewPostFields = React.useCallback(() => {
    setTitle('');
    setSlug('');
    setText(emptyEditorState());
    setCaption('');
    setMetaDescription('');
    setSeoTitle('');
    setImgSrc('');
    setPhoto('');
    setUploadSuccess(false);
    setId(0);
    setDay(`${new Date().toISOString().slice(0, 10)}T00:00:00.000Z`);
  }, []);

  React.useEffect(() => {
    if (!modal) {
      return;
    }

    if (!post) {
      setNewPostSession((s) => s + 1);
      resetNewPostFields();
      return;
    }

    const blocks = post.text ? htmlToDraft(post.text).contentBlocks : [];
    const contentState = ContentState.createFromBlockArray(blocks);
    setPhoto(post.imageSrc);
    setDay(post.date);
    setTitle(post.title ?? '');
    setSlug(post.slug ?? '');
    setText(convertToRaw(contentState));
    setCaption(post.caption ?? '');
    setMetaDescription(post.metaDescription ?? '');
    setSeoTitle(post.seoTitle ?? '');
    setImgSrc(post.imageSrc);
    setId(+post.id);
    setUploadSuccess(false);
  }, [modal, post, resetNewPostFields]);

  const handleValidation = () => {
    const errors: Record<string, string> = {};
    let formIsValid = true;

    if (!selectedDay) {
      formIsValid = false;
      errors.day =
        'Fecha de publicación es requerida. Por favor, seleccione una fecha válida.';
    }

    if (!title) {
      formIsValid = false;
      errors.title =
        'Título es requerido. Por favor, ingrese un título para el post.';
    }

    if (!slug) {
      formIsValid = false;
      errors.slug =
        'Slug es requerido. Por favor, ingrese un slug único para el post.';
    }

    if (!text) {
      formIsValid = false;
      errors.text =
        'Texto es requerido. Por favor, ingrese el contenido del post.';
    }

    if (!caption) {
      formIsValid = false;
      errors.caption =
        'Caption es requerido. Por favor, ingrese una descripción para la imagen.';
    }

    return { formIsValid, errors };
  };

  const setFile = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target?.files?.[0]) {
      setUploadSuccess(false);
      setImgSrc('');
      setPhoto(e.target.files[0]);
    }
  };

  const uploadFile = async (
    evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    evt.preventDefault();
    const formData = new FormData();
    formData.append('file', photo);

    const token = await authService.getAccessToken();

    function handleError() {
      setAlertMessage(
        'Lo sentimos, ha ocurrido un error subiendo la imagen. Por favor, inténtelo en unos minutos o pongase en contacto con el equipo técnico de UCEME para reportar el error.'
      );
      alertToggle();
    }

    const makeRequest = async (authToken: string | null | undefined) => {
      const response = await fetch(
        `${settings?.baseHref}api/blog/onpostuploadasync`,
        {
          method: 'POST',
          mode: 'cors',
          body: formData,
          headers: !authToken
            ? { Accept: 'application/json' }
            : {
                Accept: 'application/json',
                Authorization: `Bearer ${authToken}`,
              },
        }
      );

      if (response.status === 401) {
        const refreshedToken = await authService.getAccessToken();
        if (refreshedToken) {
          const retryResponse = await fetch(
            `${settings?.baseHref}api/blog/onpostuploadasync`,
            {
              method: 'POST',
              mode: 'cors',
              body: formData,
              headers: {
                Accept: 'application/json',
                Authorization: `Bearer ${refreshedToken}`,
              },
            }
          );
          if (retryResponse.status >= 200 && retryResponse.status <= 299) {
            return retryResponse.json();
          }
          handleError();
          throw Error('Session expired. Please log in again.');
        }
        handleError();
        throw Error('Session expired. Please log in again.');
      }

      if (response.status >= 200 && response.status <= 299) {
        return response.json();
      }

      handleError();
      throw Error(response.statusText);
    };

    makeRequest(token).then(async (resp: string) => {
      if (resp) {
        setImgSrc(resp);
        setUploadSuccess(true);
        setAlertMessage(`Imagen subida correctamente.`);
        alertToggle();
      } else {
        handleError();
      }
    });
  };

  const submitForm = async (
    evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    evt.preventDefault();
    const validation = handleValidation();
    if (validation.formIsValid) {
      const day = new Date(selectedDay);
      const data = {
        idBlog: id,
        titulo: title,
        slug,
        texto: draftToHtml(text),
        caption,
        fecha: day,
        seoTitle,
        metaDescription,
        foto: imgSrc,
      };

      const token = await authService.getAccessToken();
      fetch(`${settings?.baseHref}api/blog/addpost`, {
        method: 'POST',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: !token
          ? {}
          : {
              'Content-Type': 'application/json',
              Authorization: `Bearer ${token}`,
            },
        redirect: 'follow',
        referrerPolicy: 'no-referrer',
        body: JSON.stringify(data),
      })
        .then((response: { json: () => Promise<boolean> }) => response.json())
        .then(async (resp: boolean) => {
          if (resp) {
            setAlertMessage('Post registrado correctamente. Muchas gracias.');
            alertToggle();
            if (!post) {
              resetNewPostFields();
            }
            toggle();
          } else {
            setAlertMessage(
              'Lo sentimos, ha ocurrido un error registrando su post. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
            );
            alertToggle();
          }
        })
        .catch(() => {
          setAlertMessage(
            'Lo sentimos, ha ocurrido un error registrando su post. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros.'
          );
          alertToggle();
        });
    }
  };

  const editorKey = post ? post.id : `new-${newPostSession}`;

  return (
    <>
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
                    onClick={(e) => uploadFile(e)}
                    value="Subir"
                    disabled={!photo || uploadSuccess}
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
                    showClearButton={false}
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
                    key={editorKey}
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
      <Modal isOpen={alertModal} toggle={alertToggle}>
        <ModalBody>
          <section id="section-contact_form" className="container">
            <div className="row justify-content-md-center">{alertMessage}</div>
          </section>
        </ModalBody>
        <ModalFooter>
          <Button color="secondary" onClick={alertToggle}>
            Cerrar
          </Button>
        </ModalFooter>
      </Modal>
    </>
  );
}

AddPostModal.defaultProps = {
  modal: false,
  post: null,
};

export default AddPostModal;
