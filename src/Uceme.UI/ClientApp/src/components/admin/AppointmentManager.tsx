import React, { useRef } from 'react';
import { RouteComponentProps } from 'react-router';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button } from 'reactstrap';
import DeleteIcon from '@material-ui/icons/Delete';
import Appointment from '../../library/Appointment';
import AppointmentResponse from '../../library/AppointmentResponse';
import { DateTimeUtils } from '../../library/DateTimeUtils';
import './AppointmentManager.scss';
import '../appointment-sections/AppointmentModal.scss';
import SettingsContext from '../../SettingsContext';
import authService from '../api-authorization/AuthorizeService';

type AppointmentManagerState = {
  loaded: boolean;
  appointments?: Appointment[] | null;
  page?: number;
};

interface MatchParams {
  page: string;
}

type AppointmentManagerProps = RouteComponentProps<MatchParams>;

const AppointmentManager = (props: AppointmentManagerProps): JSX.Element => {
  const { match } = props;
  const [modal, setModal] = React.useState<boolean>(false);
  const toggle = () => setModal(!modal);
  const [confirmModal, setConfirmModal] = React.useState<boolean>(false);
  const confirmToggle = () => setConfirmModal(!confirmModal);
  const [alertModal, setAlertModal] = React.useState<boolean>(false);
  const alertToggle = () => setAlertModal(!alertModal);
  const [alertMessage, setAlertMessage] = React.useState<string>('');
  const [markedAppointment, setMarkedAppointment] =
    React.useState<Appointment>();
  const settings = React.useContext(SettingsContext());
  const [appointmentData, setAppointmentData] =
    React.useState<AppointmentManagerState>({
      loaded: false,
      appointments: null,
      page: +match?.params?.page ?? 1,
    });
  const [closeAppointmentData, setCloseAppointmentData] =
    React.useState<AppointmentManagerState>({
      loaded: false,
      appointments: null,
      page: +match.params?.page ?? 1,
    });

  const isFirstRun = useRef(true);

  const fetchAppointments = async (page: number) => {
    const token = await authService.getAccessToken();
    fetch(`clientapi/appointment/appointmentlist`, {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    })
      .then((response: { json: () => Promise<AppointmentResponse[]> }) =>
        response.json()
      )
      .then(async (resp: AppointmentResponse[]) => {
        const retrievedAppointments: Appointment[] = [];

        await Promise.all(
          resp.map(async (obj: AppointmentResponse) => {
            retrievedAppointments.push({
              id: obj.idCita,
              date: DateTimeUtils.FormatDate(obj.dia),
              time: DateTimeUtils.TimeToString(obj.hora),
              name: obj.nombre,
              email: obj.email,
              phone: obj.telefono,
              idTurn: obj.idTurno,
            });
          })
        );

        setAppointmentData({
          loaded: true,
          appointments: retrievedAppointments,
          page,
        });
      })
      .catch(() => {
        setAppointmentData({
          loaded: false,
          appointments: null,
          page,
        });
      });
  };

  const fetchCloseAppointments = async (page: number) => {
    const token = await authService.getAccessToken();
    fetch(`clientapi/appointment/closeappointmentlist`, {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    })
      .then((response: { json: () => Promise<AppointmentResponse[]> }) =>
        response.json()
      )
      .then(async (resp: AppointmentResponse[]) => {
        const retrievedAppointments: Appointment[] = [];

        await Promise.all(
          resp.map(async (obj: AppointmentResponse) => {
            retrievedAppointments.push({
              id: obj.idCita,
              date: DateTimeUtils.FormatDate(obj.dia),
              time: DateTimeUtils.TimeToString(obj.hora),
              name: obj.nombre,
              email: obj.email,
              phone: obj.telefono,
              idTurn: obj.idTurno,
            });
          })
        );

        setCloseAppointmentData({
          loaded: true,
          appointments: retrievedAppointments,
        });

        setModal(true);
      })
      .catch(() => {
        setAppointmentData({
          loaded: false,
          appointments: null,
          page,
        });
      });
  };

  const deleteAppointment = (appointment: Appointment) => {
    setMarkedAppointment(appointment);
    setConfirmModal(true);
  };

  const deleteMarkedAppointment = async () => {
    if (settings && markedAppointment) {
      setConfirmModal(false);
      const token = await authService.getAccessToken();
      fetch(
        `clientapi/appointment/deleteappointment?appointmentid=${+markedAppointment?.id}`,
        {
          headers: !token ? {} : { Authorization: `Bearer ${token}` },
        }
      )
        .then((response: { json: () => Promise<boolean> }) => response.json())
        .then(async (resp: boolean) => {
          if (resp === true) {
            setAlertMessage(
              'Cita previa borrada correctamente. Muchas gracias.'
            );
            alertToggle();
            setAppointmentData({
              loaded: true,
              appointments: appointmentData.appointments?.filter(
                (obj: Appointment) => obj.id !== markedAppointment?.id
              ),
              page: appointmentData.page,
            });
          } else {
            setAlertMessage(
              'Lo sentimos, ha ocurrido un error borrando su cita previa. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
            );
            alertToggle();
          }
        })
        .catch(() => {
          setAlertMessage(
            'Lo sentimos, ha ocurrido un error borrando su cita previa. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
          );
          alertToggle();
        });
    }
  };

  React.useEffect(() => {
    if (settings) {
      const page = +match.params?.page || 1;

      if (isFirstRun.current) {
        isFirstRun.current = false;

        fetchCloseAppointments(page);
        fetchAppointments(page);
        return;
      }

      setAppointmentData({ loaded: false, page });
      fetchCloseAppointments(page);
      fetchAppointments(page);
    }
  }, [match?.params?.page, settings]);

  if (appointmentData.loaded && closeAppointmentData.loaded) {
    return (
      <div className="App App-home header-distance">
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
        <Modal isOpen={confirmModal} toggle={confirmToggle}>
          <ModalBody>
            <section id="section-contact_form" className="container">
              <div className="row justify-content-md-center">
                ¿Está seguro de borrar la cita con {markedAppointment?.name}?
              </div>
            </section>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" onClick={() => deleteMarkedAppointment()}>
              Borrar
            </Button>{' '}
            <Button color="secondary" onClick={confirmToggle}>
              Cerrar
            </Button>
          </ModalFooter>
        </Modal>
        <Modal isOpen={modal} toggle={toggle} className="next-dates-modal">
          <ModalHeader toggle={toggle} className="beatabg next-dates-modal">
            <div className="Aligner next-dates-modal">
              <div className="Aligner-item Aligner-item--top" />
              <div className="Aligner-item">Citas en los próximos 2 días</div>
              <div className="Aligner-item Aligner-item--bottom" />
            </div>
          </ModalHeader>
          <ModalBody>
            <section id="section-contact_form" className="container">
              <div className="row justify-content-md-center">
                <table className="table">
                  <thead>
                    <tr>
                      <th scope="col">Fecha</th>
                      <th scope="col">Hora</th>
                      <th scope="col">Paciente</th>
                      <th scope="col">Email</th>
                      <th scope="col">Teléfono</th>
                    </tr>
                  </thead>
                  <tbody>
                    {closeAppointmentData.appointments?.map(
                      (appointment: Appointment) => {
                        return (
                          <tr key={appointment.id}>
                            <td>{appointment.date}</td>
                            <td>{appointment.time}</td>
                            <td>{appointment.name}</td>
                            <td>{appointment.email}</td>
                            <td>{appointment.phone}</td>
                          </tr>
                        );
                      }
                    )}
                  </tbody>
                </table>
              </div>
            </section>
          </ModalBody>
          <ModalFooter>
            {' '}
            <Button color="secondary" onClick={toggle}>
              Cerrar
            </Button>
          </ModalFooter>
        </Modal>
        <div className="container">
          <table className="table">
            <thead>
              <tr>
                <th scope="col">#</th>
                <th scope="col">Fecha</th>
                <th scope="col">Hora</th>
                <th scope="col">Paciente</th>
                <th scope="col">Email</th>
                <th scope="col">Teléfono</th>
                <th scope="col">Borrar</th>
              </tr>
            </thead>
            <tbody>
              {appointmentData.appointments?.map(
                (appointment: Appointment, index: number) => {
                  return (
                    <tr key={appointment.id}>
                      <th scope="row">{index}</th>
                      <td>{appointment.date}</td>
                      <td>{appointment.time}</td>
                      <td>{appointment.name}</td>
                      <td>{appointment.email}</td>
                      <td>{appointment.phone}</td>
                      <td>
                        <DeleteIcon
                          onClick={() => deleteAppointment(appointment)}
                        />
                      </td>
                    </tr>
                  );
                }
              )}
            </tbody>
          </table>
        </div>
      </div>
    );
  }

  return <div className="App App-home header-distance">Loading...</div>;
};

export default AppointmentManager;
