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
import SettingsContext from '../../SettingsContext';
import authService from '../api-authorization/AuthorizeService';
import { ContentState, convertToRaw, RawDraftContentState } from 'draft-js';
import { Editor } from 'react-draft-wysiwyg';
import draftToHtml from 'draftjs-to-html';
import './AddPostModal.scss';
import 'react-draft-wysiwyg/dist/react-draft-wysiwyg.css';

type AddPostModalProps = {
  toggle: any;
  modal?: any;
};

const AddPostModal = (props: AddPostModalProps): JSX.Element => {
  let _contentState = ContentState.createFromText('');
  const raw = convertToRaw(_contentState)
  
  const settings = React.useContext(SettingsContext());
  const inputName = 'reactstrap_date_picker_basic';
  const [photo, setPhoto] = React.useState<any>(null);
  const [selectedDay, setDay] = React.useState<string>(
    `${new Date().toISOString().slice(0, 10)}T00:00:00.000Z`
  );
  const [title, setTitle] = React.useState<string>();
  const [slug, setSlug] = React.useState<string>();
  const [text, setText] = React.useState<RawDraftContentState>(raw);
  const [caption, setCaption] = React.useState<string>();
  const [metadescription, setMetadescription] = React.useState<string>();
  const [seoTitle, setSeoTitle] = React.useState<string>();
  const [imgSrc, setImgSrc] = React.useState<string>();
  
  const weekStart = 1;

  const resetForm = () => {
    setTitle(undefined);
    setSlug(undefined);
    setText(raw);
    setCaption(undefined);
  };

  const handleValidation = () => {
    const errors: any = {};
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

  const setFile = (e: any) => {
    setPhoto(e.target.files[0]);
  }

  const uploadfile = async (evt: any) => {
    evt.preventDefault();
    const formData = new FormData();
    formData.append('file', photo);
    
    const token = await authService.getAccessToken();

    try {
      fetch(`clientapi/blog/onpostuploadasync`, {
        method: 'POST',
        mode: 'cors',
        body: formData,
        headers: !token ?
        { 'Accept': 'application/json', } :
        { 'Accept': 'application/json', 'Authorization': `Bearer ${token}` },
      })
      .then((response: { json: () => any }) => response.json())
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
      .catch((error: any) => {
        alert(
          'Lo sentimos, ha ocurrido un error subiendo la imagen. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros.'
        );

        console.log(error);
      });
    } catch (error) {
      console.error('Error:', error);
    }
  }

  const submitForm = (evt: any) => {
    evt.preventDefault();
    debugger;
    if (handleValidation() && settings) {
      const day = new Date(selectedDay);
      const data = {
        titulo: title,
        slug: slug,
        texto: draftToHtml(text),
        caption: caption,
        fecha: day,
        seoTitle: seoTitle,
        metadescription: metadescription,
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
        .then((response: { json: () => any }) => response.json())
        .then(async (resp: any) => {
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
        .catch((error: any) => {
          alert(
            'Lo sentimos, ha ocurrido un error registrando su post. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros.'
          );
          console.log(error);
        });
    }
  };

  return (
    <Modal isOpen={props.modal} toggle={props.toggle}>
      <ModalHeader toggle={props.toggle} className="beatabg">
        <div className="Aligner">
          <div className="Aligner-item Aligner-item--top" />
          <div className="Aligner-item">Nuevo Post</div>
          <div className="Aligner-item Aligner-item--bottom" />
        </div>
      </ModalHeader>
      <ModalBody>
        <section id="section-contact_form" className="container">
          <div className="row justify-content-md-center">
            <form className="col-12">
              <div className="extra-padding field-margin">
                <Label for="FileUpload_FormFile">Foto</Label>
                <Input id="FileUpload_FormFile" type="file" 
                    name="FileUpload.FormFile"
                    onChange={(e) => setFile(e)} />
                <Button className="submit-form-button" onClick={e => uploadfile(e)} value="Subir">
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
                    onChange={(v: any) => {
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
                    onChange={(evt) => setCaption(evt.target.value)}
                  />
                  <Label for="metaForm" className="field-label">
                    Meta - description
                  </Label>
                  <Input
                    type="textarea"
                    name="metaForm"
                    id="metaForm"
                    onChange={(evt) => setMetadescription(evt.target.value)}
                  />
                  <Label for="seoForm" className="field-label">
                    Titulo para SEO
                  </Label>
                  <Input
                    type="textarea"
                    name="seoForm"
                    id="seoForm"
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
        >
          Publicar
        </Button>{' '}
        <Button color="secondary" onClick={props.toggle}>
          Cancelar
        </Button>
      </ModalFooter>
    </Modal>
  );
};

export default AddPostModal;
