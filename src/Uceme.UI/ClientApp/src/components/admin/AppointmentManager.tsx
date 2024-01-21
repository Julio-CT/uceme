import React, { ReactElement } from 'react';
import { useParams } from 'react-router-dom';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button } from 'reactstrap';
import DeleteIcon from '@mui/icons-material/Delete';
import Appointment from '../../library/Appointment';
import AppointmentResponse from '../../library/AppointmentResponse';
import { DateTimeUtils } from '../../library/DateTimeUtils';
import './AppointmentManager.scss';
import '../appointment-sections/AppointmentModal.scss';
import authService from '../api-authorization/AuthorizeService';
import SettingsContext, { Settings } from '../../SettingsContext';

type AppointmentManagerState = {
  loaded: boolean;
  appointments?: Appointment[] | null;
  page?: number;
};

function AppointmentManager(): ReactElement {
  const { page } = useParams();
  const settings: Settings = React.useContext(SettingsContext);
  const [closeAppointmentsModal, setCloseAppointmentsModal] =
    React.useState<boolean>(false);
  const closeAppointmentsToggle = () =>
    setCloseAppointmentsModal(!closeAppointmentsModal);
  const [confirmModal, setConfirmModal] = React.useState<boolean>(false);
  const confirmToggle = () => setConfirmModal(!confirmModal);
  const [alertModal, setAlertModal] = React.useState<boolean>(false);
  const alertToggle = () => setAlertModal(!alertModal);
  const [alertMessage, setAlertMessage] = React.useState<string>('');
  const [markedAppointment, setMarkedAppointment] =
    React.useState<Appointment>();
  const [appointmentData, setAppointmentData] =
    React.useState<AppointmentManagerState>({
      loaded: false,
      appointments: null,
      page: page ? +page : 1,
    });
  const [closeAppointmentData, setCloseAppointmentData] =
    React.useState<AppointmentManagerState>({
      loaded: false,
      appointments: null,
      page: page ? +page : 1,
    });

  const isFirstRun = React.useRef(true);

  const deleteAppointment = (appointment: Appointment) => {
    setMarkedAppointment(appointment);
    setConfirmModal(true);
  };

  const deleteMarkedAppointment = async () => {
    if (markedAppointment) {
      setConfirmModal(false);
      const token = await authService.getAccessToken();
      fetch(
        `${settings?.baseHref}
        api/appointment/deleteappointment?appointmentid=${+markedAppointment.id}`,
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
              'Lo sentimos, ha ocurrido un error borrando la cita previa. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
            );
            alertToggle();
          }
        })
        .catch(() => {
          setAlertMessage(
            'Lo sentimos, ha ocurrido un error borrando la cita previa. Por favor, inténtelo en unos minutos o pongase en contacto por teléfono con nosotros..'
          );
          alertToggle();
        });
    }
  };

  React.useEffect(() => {
    const currentPage = page ? +page : 1;

    const fetchAppointments = async (pageToFetch: number) => {
      const token = await authService.getAccessToken();
      if (settings?.baseHref !== undefined) {
        fetch(`${settings?.baseHref}api/appointment/appointmentlist`, {
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
                  speciality: obj.speciality,
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
              page: pageToFetch,
            });
          })
          .catch(() => {
            setAppointmentData({
              loaded: false,
              appointments: null,
              page: pageToFetch,
            });
          });
      }
    };

    const fetchCloseAppointments = async (pageToFetch: number) => {
      const token = await authService.getAccessToken();

      if (settings?.baseHref !== undefined) {
        fetch(`${settings?.baseHref}api/appointment/closeappointmentlist`, {
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
                  speciality: obj.speciality,
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

            setCloseAppointmentsModal(true);
          })
          .catch(() => {
            setAppointmentData({
              loaded: false,
              appointments: null,
              page: pageToFetch,
            });
          });
      }
    };

    const updatePastAppointmentsData = async () => {
      const token = await authService.getAccessToken();

      if (settings?.baseHref !== undefined) {
        fetch(
          `${settings?.baseHref}api/appointment/updatepastappointmentsData`,
          {
            headers: !token ? {} : { Authorization: `Bearer ${token}` },
          }
        )
          .then((response: { json: () => Promise<boolean> }) => response.json())
          .catch();
      }
    };

    if (isFirstRun.current) {
      isFirstRun.current = false;

      fetchCloseAppointments(currentPage);
      fetchAppointments(currentPage);
      updatePastAppointmentsData();
      return;
    }

    setAppointmentData({ loaded: false, page: currentPage });
    fetchCloseAppointments(currentPage);
    fetchAppointments(currentPage);
  }, [page, settings?.baseHref]);

  if (appointmentData.loaded && closeAppointmentData.loaded) {
    return (
      <div className="app app-home header-distance-l">
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
                ¿Está seguro de eliminar la cita con {markedAppointment?.name}?
              </div>
            </section>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" onClick={() => deleteMarkedAppointment()}>
              Eliminar
            </Button>{' '}
            <Button color="secondary" onClick={confirmToggle}>
              Cerrar
            </Button>
          </ModalFooter>
        </Modal>
        <Modal
          isOpen={closeAppointmentsModal}
          toggle={closeAppointmentsToggle}
          className="next-dates-modal"
        >
          <ModalHeader
            toggle={closeAppointmentsToggle}
            className="beatabg next-dates-modal"
          >
            <div className="aligner next-dates-modal">
              <div className="aligner-item aligner-item-top" />
              <div className="aligner-item">Citas en los próximos 2 días</div>
              <div className="aligner-item aligner-item-bottom" />
            </div>
          </ModalHeader>
          <ModalBody>
            <section id="section-contact_form" className="container">
              <div className="row justify-content-md-center">
                <table className="table">
                  <thead>
                    <tr>
                      <th scope="col">Especialidad</th>
                      <th scope="col">Fecha</th>
                      <th scope="col">Hora</th>
                      <th scope="col">Paciente</th>
                      <th scope="col">Teléfono</th>
                    </tr>
                  </thead>
                  <tbody>
                    {closeAppointmentData.appointments?.map(
                      (appointment: Appointment) => {
                        return (
                          <tr key={appointment.id}>
                            <td>{appointment.speciality}</td>
                            <td>{appointment.date}</td>
                            <td>{appointment.time}</td>
                            <td>{appointment.name}</td>
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
            <Button color="secondary" onClick={closeAppointmentsToggle}>
              Cerrar
            </Button>
          </ModalFooter>
        </Modal>
        <div className="container">
          <table className="table">
            <thead>
              <tr>
                <th scope="col">Especialidad</th>
                <th scope="col">Fecha</th>
                <th scope="col">Hora</th>
                <th scope="col">Paciente</th>
                <th scope="col">Email</th>
                <th scope="col">Teléfono</th>
                <th scope="col">Eliminar</th>
              </tr>
            </thead>
            <tbody>
              {appointmentData.appointments?.map((appointment: Appointment) => {
                return (
                  <tr key={appointment.id}>
                    <td>{appointment.speciality}</td>
                    <td>{appointment.date}</td>
                    <td>{appointment.time}</td>
                    <td>{appointment.name}</td>
                    <td>{appointment.email}</td>
                    <td>{appointment.phone}</td>
                    <td>
                      <DeleteIcon
                        aria-label="Delete"
                        onClick={() => deleteAppointment(appointment)}
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

export default AppointmentManager;
