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
import AppointmentHours from './appointment-sections/AppointmentHours';
import DatePicker from 'reactstrap-date-picker2';
import SettingsContext from '../SettingsContext';
import './AppointmentModal.scss';

const AppointmentModal = (props: any): JSX.Element => {
  const settings = React.useContext(SettingsContext());
  const inputName = 'reactstrap_date_picker_basic';
  const [showDays, setShowDays] = React.useState<boolean>(true);
  const [showHours, setShowHours] = React.useState<boolean>(false);
  const [sendEnabled, setSendEnabled] = React.useState<boolean>(false);
  const [disabledDays, setDisabledDays] = React.useState<number[]>([
    0,
    1,
    2,
    3,
    4,
    5,
    6,
  ]);
  const [daysFetched, setDaysFetched] = React.useState<boolean>(false);
  const [selectedDay, setDay] = React.useState<string>(
    `${new Date().toISOString().slice(0, 10)}T00:00:00.000Z`
  );
  const [hours, setHours] = React.useState<string[]>([]);
  const [selectedHour, setSelectedHour] = React.useState<string>();
  const [email, setEmail] = React.useState<string>();
  const [name, setName] = React.useState<string>();
  const [phone, setPhone] = React.useState<string>();
  const [extraInfo, setExtraInfo] = React.useState<string>();
  const [acceptTC, setAcceptTC] = React.useState<boolean>(false);
  const weekStart = 1;
  const hospitalId = '1';
  const hospital = 'Beata María Ana';

  const resetForm = () => {
    setShowHours(false);
    setSendEnabled(false);
    setAcceptTC(false);
    setName(undefined);
    setPhone(undefined);
    setEmail(undefined);
    setExtraInfo(undefined);
  };

  const fetchDays = React.useCallback(
    (hospital: string, baseHref: string) => {
      if (!daysFetched) {
        fetch(`${baseHref}api/appointment/getdays?hospitalId=${hospital}`)
          .then((response: { json: () => any }) => response.json())
          .then(async (resp: any) => {
            setDisabledDays(
              disabledDays.filter((el) => !resp.includes(el + 1))
            );
            resetForm();
            setShowDays(true);
            setDaysFetched(true);
          })
          .catch((error: any) => {
            console.log(error);
          });
      }
    },
    [disabledDays, daysFetched]
  );

  const fetchHours = (date: string, baseHref: string) => {
    const day = new Date(date);
    const data = {
      weekDay: day.getDay(),
      hospitalId: hospitalId,
      day: day.getDate(),
      month: day.getMonth() + 1,
      year: day.getFullYear(),
    };

    fetch(`${baseHref}api/appointment/gethours`, {
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
        setShowHours(true);
        setHours(resp.hours);
        setSendEnabled(false);
      })
      .catch((error: any) => {
        console.log(error);
        setHours([]);
        setSendEnabled(false);
      });
  };

  const selectHour = (hour: string) => {
    setSelectedHour(hour);
    setSendEnabled(true);
  };

  const selectDay = (value: any, frmValue: any) => {
    if (settings) {
      fetchHours(value, settings.baseHref);
      setDay(value);
    }
  };

  const handleValidation = () => {
    let errors: any = {};
    let formIsValid = true;

    //Name
    if (!selectedDay) {
      formIsValid = false;
      errors['day'] = 'Cannot be empty';
    }

    if (!selectedHour) {
      formIsValid = false;
      errors['hour'] = 'Cannot be empty';
    }

    //Email
    if (!email) {
      formIsValid = false;
      errors['email'] = 'Cannot be empty';
    }

    if (typeof email !== 'undefined') {
      let lastAtPos = email.lastIndexOf('@');
      let lastDotPos = email.lastIndexOf('.');

      if (
        !(
          lastAtPos < lastDotPos &&
          lastAtPos > 0 &&
          email.indexOf('@@') === -1 &&
          lastDotPos > 2 &&
          email.length - lastDotPos > 2
        )
      ) {
        formIsValid = false;
        errors['email'] = 'Email is not valid';
      }
    }

    if (!name) {
      formIsValid = false;
      errors['name'] = 'Cannot be empty';
    }

    if (!phone) {
      formIsValid = false;
      errors['phone'] = 'Cannot be empty';
    }

    if (!acceptTC) {
      formIsValid = false;
      errors['acceptTC'] = 'Cannot be empty';
    }

    return formIsValid;
  };

  const submitForm = () => {
    if (handleValidation() && settings) {
      const day = new Date(selectedDay);
      const data = {
        weekDay: day.getDay(),
        hospitalId: hospitalId,
        day: day.getDate(),
        month: day.getMonth() + 1,
        year: day.getFullYear(),
        hour: selectedHour,
        name: name,
        phone: phone,
        email: email,
        extraInfo: extraInfo,
      };

      fetch(`${settings.baseHref}api/appointment/addappointment`, {
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
          if (resp.status === 200) {
            alert('Cita previa registrada correctamente. Muchas gracias.');
            resetForm();
            props.toggle();
          } else {
            alert(
              'Lo sentimos, ha ocurrido un error registrando su cita previa. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
            );
          }
        })
        .catch((error: any) => {
          alert(
            'Lo sentimos, ha ocurrido un error registrando su cita previa. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
          );
          console.log(error);
        });
    }
  };

  React.useEffect(() => {
    if (settings) {
      fetchDays(hospitalId, settings.baseHref);
    }
  }, [settings, fetchDays]);

  return (
    <Modal isOpen={props.modal} toggle={props.toggle}>
      <ModalHeader toggle={props.toggle} className="beatabg">
        <div className="Aligner">
          <div className="Aligner-item Aligner-item--top"></div>
          <div className="Aligner-item">Reserve cita</div>
          <div className="Aligner-item Aligner-item--bottom"></div>
        </div>
      </ModalHeader>
      <ModalBody>
        <section id="section-contact_form" className="container">
          <div className="row justify-content-md-center">
            <form className="col-12">
              <span className="field-margin">Hospital {hospital}</span>
              {showDays && (
                <div className="extra-padding field-margin">
                  <Label for="dateForm" className="field-label">
                    Fecha
                  </Label>
                  <DatePicker
                    id="dateForm"
                    name={inputName}
                    value={selectedDay}
                    onChange={(v: any, f: any) => {
                      selectDay(v, f);
                    }}
                    disabledWeekDays={disabledDays}
                    weekStartsOn={weekStart}
                    minDate={`${new Date()
                      .toISOString()
                      .slice(0, 10)}T00:00:00.000Z`}
                  />
                </div>
              )}
              {showHours && (
                <div>
                  <Label for="hourForm" className="field-label field-margin">
                    Hora
                  </Label>
                  <AppointmentHours
                    hours={hours}
                    onSelectedHour={(v: any) => selectHour(v)}
                  />
                </div>
              )}
              {sendEnabled && (
                <div className="field-margin">
                  <Label for="nameForm" className="field-label">
                    Nombre
                  </Label>
                  <Input
                    type="text"
                    name="nameForm"
                    id="nameForm"
                    placeholder="Campo requerido"
                    onChange={(evt) => setName(evt.target.value)}
                    required
                  />
                  <Label for="phoneForm" className="field-label">
                    Teléfono
                  </Label>
                  <Input
                    type="tel"
                    name="phoneForm"
                    id="phoneForm"
                    placeholder="Campo requerido"
                    onChange={(evt) => setPhone(evt.target.value)}
                    required
                  />
                  <Label for="emailForm" className="field-label">
                    Email de contacto
                  </Label>
                  <Input
                    type="email"
                    name="emailForm"
                    id="emailForm"
                    placeholder="Campo requerido"
                    onChange={(evt) => setEmail(evt.target.value)}
                    required
                  />
                  <Label for="notesForm" className="field-label">
                    Observaciones
                  </Label>
                  <Input
                    type="textarea"
                    name="notes"
                    id="notesForm"
                    onChange={(evt) => setExtraInfo(evt.target.value)}
                  />
                </div>
              )}
              {name && email && phone && (
                <div>
                  <Label check>
                    <Input
                      type="checkbox"
                      checked={acceptTC}
                      onChange={() => setAcceptTC(!acceptTC)}
                    />{' '}
                    Acepto la{' '}
                    <a
                      href="https://www.pakainfo.com"
                      target="_blank"
                      rel="noopener noreferrer"
                    >
                      cláusula de protección de datos
                    </a>
                  </Label>
                </div>
              )}
            </form>
          </div>
        </section>
      </ModalBody>
      <ModalFooter>
        <Button
          className="submit-form-button"
          disabled={!sendEnabled || !name || !email || !phone || !acceptTC}
          onClick={() => submitForm()}
        >
          Confirmar cita
        </Button>{' '}
        <Button color="secondary" onClick={props.toggle}>
          Cancelar
        </Button>
      </ModalFooter>
    </Modal>
  );
};

export default AppointmentModal;
