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
import SettingsContext, { Settings } from '../../SettingsContext';
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

function AppointmentModal(props: AppointmentModalProps): JSX.Element {
  const { modal, toggle } = props;

  const [alertModal, setAlertModal] = React.useState<boolean>(false);
  const alertToggle = () => setAlertModal(!alertModal);
  const [alertMessage, setAlertMessage] = React.useState<string>('');
  const [error, setError] = React.useState<string | null>(null);

  const settings: Settings = React.useContext(SettingsContext);
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

  const [validationErrors, setValidationErrors] = React.useState<{
    day: string;
    hour: string;
    email: string;
    name: string;
    phone: string;
    acceptTC: string;
  }>({
    day: '',
    hour: '',
    email: '',
    name: '',
    phone: '',
    acceptTC: '',
  });

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
    async (baseHref: string) => {
      if (!hospitalsFetched) {
        try {
          const response = await fetch(`${baseHref}api/hospital`);
          if (!response.ok) {
            throw new Error(`Error fetching hospitals: ${response.statusText}`);
          }

          const resp: Hospital[] = await response.json();
          resetForm();
          setHospitals(resp);
          setShowHospitals(true);
          setDisabledDays([0, 1, 2, 3, 4, 5, 6]);
          setHospitalsFetched(true);
          setError(null);
        } catch (err) {
          setError('Error loading hospitals. Please try again later.');
          setHospitalsFetched(false);
        }
      }
    },
    [hospitalsFetched]
  );

  const fetchDays = async (
    hospital: string,
    baseHref: string,
    forceFetch: boolean
  ) => {
    if (!daysFetched || forceFetch) {
      try {
        const response = await fetch(
          `${baseHref}api/appointment/days/${hospital}`
        );
        if (!response.ok) {
          throw new Error(`Error fetching days: ${response.statusText}`);
        }

        const resp: number[] = await response.json();
        setDisabledDays(
          [0, 1, 2, 3, 4, 5, 6].filter((el) => !resp.includes(el + 1))
        );
        setShowDays(true);
        setDaysFetched(true);
        setError(null);
      } catch (err) {
        setError('Error loading available days. Please try again later.');
        setDaysFetched(false);
      }
    }
  };

  const fetchHours = async (date: string, baseHref: string) => {
    const day = new Date(date);
    const params = new URLSearchParams({
      weekDay: day.getDay().toString(),
      hospitalId: hospitalId?.toString() || '',
      day: day.getDate().toString(),
      month: (day.getMonth() + 1).toString(),
      year: day.getFullYear().toString(),
    });

    try {
      const response = await fetch(
        `${baseHref}api/appointment/hours?${params.toString()}`
      );

      if (!response.ok) {
        throw new Error(`Error fetching hours: ${response.statusText}`);
      }

      const resp: AppointmentHoursResponse = await response.json();
      setHours(resp.hours);
      setSendEnabled(false);
      setShowHours(true);
      setError(null);
    } catch (err) {
      setError('Error loading available hours. Please try again later.');
      setHours([]);
      setShowHours(false);
      setSendEnabled(false);
    }
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
      errors.day = 'Este campo es obligatorio';
    }

    if (!selectedHour) {
      formIsValid = false;
      errors.hour = 'Este campo es obligatorio';
    }

    if (!email) {
      formIsValid = false;
      errors.email = 'Este campo es obligatorio';
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
        errors.email = 'El email no es válido';
      }
    }

    if (!name) {
      formIsValid = false;
      errors.name = 'Este campo es obligatorio';
    } else if (name.length < 4) {
      formIsValid = false;
      errors.name = 'El nombre debe tener al menos 4 caracteres';
    } else if (!name.includes(' ')) {
      formIsValid = false;
      errors.name = 'Por favor, introduce nombre y apellidos';
    } else if (!/^[a-zA-ZÀ-ÿ\s]*$/.test(name)) {
      formIsValid = false;
      errors.name = 'El nombre no puede contener caracteres especiales';
    }

    if (!phone) {
      formIsValid = false;
      errors.phone = 'Este campo es obligatorio';
    } else if (!/^[0-9+\s]+$/.test(phone)) {
      formIsValid = false;
      errors.phone =
        'El teléfono solo puede contener números, espacios y el símbolo "+"';
    }

    if (!acceptTC) {
      formIsValid = false;
      errors.acceptTC = 'Este campo es obligatorio';
    }

    setValidationErrors(errors);
    return formIsValid;
  };

  const submitForm = async () => {
    if (handleValidation() && settings) {
      const day = new Date(selectedDay);

      // Sanitize user inputs to prevent security issues
      const sanitizedData = {
        weekDay: day.getDay(),
        hospitalId: parseInt(hospitalId || '0', 10),
        day: day.getDate(),
        month: day.getMonth() + 1,
        year: day.getFullYear(),
        hour: selectedHour,
        name: name?.trim().substring(0, 100) || '', // Limit length and trim
        phone: phone?.trim().substring(0, 20) || '', // Limit length and trim
        email: email?.trim().toLowerCase().substring(0, 100) || '', // Limit length, trim, and lowercase
        extraInfo: extraInfo ? extraInfo.trim().substring(0, 500) : '', // Optional field with length limit
      };

      try {
        // console.log('Submitting appointment data:', sanitizedData);
        const response = await fetch(
          `${settings.baseHref}api/appointment/addappointment`,
          {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
            },
            body: JSON.stringify(sanitizedData),
          }
        );

        if (!response.ok) {
          const errorText = await response.text();
          // console.error('Server error response:', errorText);
          throw new Error(
            `Error creating appointment: ${response.statusText}. ${errorText}`
          );
        }

        const resp: boolean = await response.json();
        if (resp) {
          setAlertMessage(
            'Cita previa registrada correctamente. Recibirá un email con la confimación. Muchas gracias.'
          );
        } else {
          setAlertMessage(
            'Cita previa registrada correctamente. El envio del correo con la confimación ha fallado, pero su cita queda registrada. Muchas gracias.'
          );
        }
        alertToggle();
        resetForm();
        toggle();
        setError(null);
      } catch (err) {
        // console.error('Error submitting appointment:', err);
        setError(
          err instanceof Error
            ? err.message
            : 'Error registering appointment. Please try again later.'
        );
        setAlertMessage(
          'Lo sentimos, ha ocurrido un error registrando tu cita previa. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros.'
        );
        alertToggle();
      }
    }
  };

  React.useEffect(() => {
    if (settings && modal) {
      fetchHospitals(settings.baseHref);
    }
  }, [settings, modal, fetchHospitals]);

  return (
    <>
      <Modal isOpen={modal} toggle={toggle}>
        <ModalHeader className="beatabg">
          <div className="aligner">
            <div className="aligner-item aligner-item-top" />
            <div className="aligner-item">Reserva cita</div>
            <div className="aligner-item aligner-item-bottom" />
          </div>
        </ModalHeader>
        <ModalBody>
          {error && (
            <div className="alert alert-danger" role="alert">
              {error}
            </div>
          )}
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
                      showClearButton={false}
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
                      Nombre completo
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
          {Object.values(validationErrors).some(
            (errorMsg) => errorMsg !== ''
          ) && (
            <div className="validation-errors mt-3">
              {Object.entries(validationErrors).map(
                ([field, message]) =>
                  message && (
                    <div key={field} className="text-danger mb-1">
                      {field}: {message}
                    </div>
                  )
              )}
            </div>
          )}
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

AppointmentModal.defaultProps = {
  modal: false,
};

export default AppointmentModal;
