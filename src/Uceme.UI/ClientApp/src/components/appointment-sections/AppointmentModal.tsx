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
  ButtonGroup,
} from 'reactstrap';
import DatePicker from 'reactstrap-date-picker2';
import AppointmentHours from './AppointmentHours';
import SettingsContext from '../../SettingsContext';
import './AppointmentModal.scss';

type Hospital = {
  idDatosPro: string;
  nombre?: string;
};

type AppointmentModalProps = {
  toggle: () => void;
  modal?: boolean;
};

type AppointmentHoursResponse = {
  hours: string[];
};

const AppointmentModal = (props: AppointmentModalProps): JSX.Element => {
  const settings = React.useContext(SettingsContext());
  const inputName = 'reactstrap_date_picker_basic';
  const [showHospitals, setShowHospitals] = React.useState<boolean>(true);
  const [showDays, setShowDays] = React.useState<boolean>(false);
  const [showHours, setShowHours] = React.useState<boolean>(false);
  const [sendEnabled, setSendEnabled] = React.useState<boolean>(false);
  const defaultDisabledDays = [0, 1, 2, 3, 4, 5, 6];
  const [disabledDays, setDisabledDays] =
    React.useState<number[]>(defaultDisabledDays);
  const [hospitalsFetched, setHospitalsFetched] =
    React.useState<boolean>(false);
  const [daysFetched, setDaysFetched] = React.useState<boolean>(false);
  const [hospitalId, setHospitalId] = React.useState<string>();
  const [hospitals, setHospitals] = React.useState<Hospital[]>();
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
  const hospitalName = 'Beata María Ana';

  const resetForm = () => {
    setShowHours(false);
    setSendEnabled(false);
    setAcceptTC(false);
    setName(undefined);
    setPhone(undefined);
    setEmail(undefined);
    setExtraInfo(undefined);
  };

  const fetchHospitals = React.useCallback(
    (baseHref: string) => {
      if (!hospitalsFetched) {
        fetch(`${baseHref}api/hospital/gethospitals`)
          .then((response: { json: () => Promise<Hospital[]> }) =>
            response.json()
          )
          .then(async (resp: Hospital[]) => {
            resetForm();
            setHospitals(resp);
            setShowHospitals(true);
            setDisabledDays([0, 1, 2, 3, 4, 5, 6]);
            setHospitalsFetched(true);
          });
      }
    },
    [hospitalsFetched]
  );

  const fetchDays = (
    hospital: string,
    baseHref: string,
    forceFetch: boolean
  ) => {
    if (!daysFetched || forceFetch) {
      fetch(`${baseHref}api/appointment/getdays?hospitalId=${hospital}`)
        .then((response: { json: () => Promise<number[]> }) => response.json())
        .then(async (resp: number[]) => {
          setDisabledDays(
            [0, 1, 2, 3, 4, 5, 6].filter((el) => !resp.includes(el + 1))
          );
          setShowDays(true);
          setDaysFetched(true);
        });
    }
  };

  const fetchHours = (date: string, baseHref: string) => {
    const day = new Date(date);
    const data = {
      weekDay: day.getDay(),
      hospitalId: hospitalId?.toString(),
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
      .then((response: { json: () => Promise<AppointmentHoursResponse> }) =>
        response.json()
      )
      .then(async (resp: AppointmentHoursResponse) => {
        setHours(resp.hours);
        setSendEnabled(false);
        setShowHours(true);
      })
      .catch(() => {
        setHours([]);
        setShowHours(false);
        setSendEnabled(false);
      });
  };

  const selectHospital = (hospital: string, forceFetch: boolean) => {
    if (settings) {
      setHospitalId(hospital);
      fetchDays(hospital, settings.baseHref, forceFetch);
      setShowHours(false);
      setSendEnabled(false);
    }
  };

  const selectHour = (hour: string) => {
    setSelectedHour(hour);
    setSendEnabled(true);
  };

  const selectDay = (value: string) => {
    if (settings) {
      fetchHours(value, settings.baseHref);
      setDay(value);
    }
  };

  const handleValidation = () => {
    const errors = {
      day: '',
      hour: '',
      email: '',
      name: '',
      phone: '',
      acceptTC: '',
    };

    let formIsValid = true;

    if (!selectedDay) {
      formIsValid = false;
      errors.day = 'Cannot be empty';
    }

    if (!selectedHour) {
      formIsValid = false;
      errors.hour = 'Cannot be empty';
    }

    if (!email) {
      formIsValid = false;
      errors.email = 'Cannot be empty';
    }

    if (typeof email !== 'undefined') {
      const lastAtPos = email.lastIndexOf('@');
      const lastDotPos = email.lastIndexOf('.');

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
        errors.email = 'Email is not valid';
      }
    }

    if (!name) {
      formIsValid = false;
      errors.name = 'Cannot be empty';
    }

    if (!phone) {
      formIsValid = false;
      errors.phone = 'Cannot be empty';
    }

    if (!acceptTC) {
      formIsValid = false;
      errors.acceptTC = 'Cannot be empty';
    }

    return formIsValid;
  };

  const submitForm = () => {
    if (handleValidation() && settings) {
      const day = new Date(selectedDay);
      const data = {
        weekDay: day.getDay(),
        hospitalId,
        day: day.getDate(),
        month: day.getMonth() + 1,
        year: day.getFullYear(),
        hour: selectedHour,
        name,
        phone,
        email,
        extraInfo,
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
        .then((response: { json: () => Promise<boolean> }) => response.json())
        .then(async (resp: boolean) => {
          if (resp === true) {
            alert('Cita previa registrada correctamente. Muchas gracias.');
            resetForm();
            props.toggle();
          } else {
            alert(
              'Lo sentimos, ha ocurrido un error registrando su cita previa. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
            );
          }
        })
        .catch(() => {
          alert(
            'Lo sentimos, ha ocurrido un error registrando su cita previa. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros.'
          );
        });
    }
  };

  const { modal, toggle } = props;

  React.useEffect(() => {
    if (settings && modal) {
      fetchHospitals(settings.baseHref);
    }
  }, [settings, modal, fetchHospitals]);

  return (
    <Modal isOpen={modal} toggle={toggle}>
      <ModalHeader className="beatabg">
        <div className="Aligner">
          <div className="Aligner-item Aligner-item--top" />
          <div className="Aligner-item">Reserve cita</div>
          <div className="Aligner-item Aligner-item--bottom" />
        </div>
      </ModalHeader>
      <ModalBody>
        <section id="section-contact_form" className="container">
          <div className="row justify-content-md-center">
            <form className="col-12">
              <span className="field-margin">Hospital {hospitalName}</span>
              {showHospitals && hospitals && (
                <div className="extra-padding field-margin">
                  <Label for="dateForm" className="field-label">
                    Servicio
                  </Label>
                  <br />
                  <ButtonGroup id="serviceForm">
                    {hospitals.map((hospital: Hospital) => {
                      return (
                        <Button
                          key={hospital.idDatosPro}
                          active={hospitalId === hospital.idDatosPro}
                          onClick={() =>
                            selectHospital(hospital.idDatosPro, true)
                          }
                          className="hospital-name"
                        >
                          {hospital.nombre}
                        </Button>
                      );
                    })}
                  </ButtonGroup>
                </div>
              )}
              {showDays && (
                <div className="field-margin">
                  <Label for="dateForm" className="field-label">
                    Fecha
                  </Label>
                  <DatePicker
                    id="dateForm"
                    name={inputName}
                    value={selectedDay}
                    onChange={(v: string) => {
                      selectDay(v);
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
                    onSelectedHour={(v: string) => selectHour(v)}
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
                <div className="field-margin">
                  <Label check>
                    <Input
                      type="checkbox"
                      checked={acceptTC}
                      onChange={() => setAcceptTC(!acceptTC)}
                    />{' '}
                    Acepto la{' '}
                    <a
                      href="condiciones"
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
        <Button color="secondary" onClick={toggle}>
          Cancelar
        </Button>
      </ModalFooter>
    </Modal>
  );
};

AppointmentModal.defaultProps = {
  modal: false,
};

export default AppointmentModal;
