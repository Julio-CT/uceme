import React, { useRef } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button } from 'reactstrap';
import Appointment from '../library/Appointment';
import { DateTimeUtils } from '../library/DateTimeUtils';
import './AppointmentManager.scss';
import './AppointmentModal.scss';
import SettingsContext from '../SettingsContext';
import authService from './api-authorization/AuthorizeService';

type AppointmentManagerState = {
  loaded: boolean;
  appointments?: any;
  page?: number;
};

type AppointmentManagerProps = {
  children: React.ReactElement[];
  params?: any;
  history?: any;
  location?: any;
  match?: any;
};

const AppointmentManager = (props: AppointmentManagerProps) => {
  const [modal, setModal] = React.useState(false);
  const toggle = () => setModal(!modal);
  const settings = React.useContext(SettingsContext());
  const [
    appointmentData,
    setAppointmentData,
  ] = React.useState<AppointmentManagerState>({
    loaded: false,
    appointments: null,
    page: props?.params?.page ?? props?.match?.params?.page ?? 1,
  });
  const [
    closeAppointmentData,
    setCloseAppointmentData,
  ] = React.useState<AppointmentManagerState>({
    loaded: false,
    appointments: null,
    page: props?.params?.page ?? props?.match?.params?.page ?? 1,
  });

  const isFirstRun = useRef(true);

  const fetchAppointments = async (page: number, baseHref: string) => {
    const token = await authService.getAccessToken();
    fetch(`clientapi/appointment/appointmentlist`, {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    })
      .then((response: { json: () => any }) => response.json())
      .then(async (resp: any) => {
        const retrievedAppointments: Appointment[] = [];

        await Promise.all(
          resp.map(async (obj: any) => {
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
          page: page,
        });
      })
      .catch((error: any) => {
        console.log(error);
        setAppointmentData({
          loaded: false,
          appointments: null,
          page: page,
        });
      });
  };

  const fetchCloseAppointments = async (page: number, baseHref: string) => {
    const token = await authService.getAccessToken();
    fetch(`clientapi/appointment/closeappointmentlist`, {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    })
      .then((response: { json: () => any }) => response.json())
      .then(async (resp: any) => {
        const retrievedAppointments: Appointment[] = [];

        await Promise.all(
          resp.map(async (obj: any) => {
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
      .catch((error: any) => {
        console.log(error);
        setAppointmentData({
          loaded: false,
          appointments: null,
          page: page,
        });
      });
  };

  React.useEffect(() => {
    if (settings) {
      const page = props?.params?.page || props?.match?.params?.page || 1;

      if (isFirstRun.current) {
        isFirstRun.current = false;

        fetchCloseAppointments(page, settings.baseHref);
        fetchAppointments(page, settings.baseHref);
        return;
      }

      setAppointmentData({ loaded: false, page: page });
      fetchCloseAppointments(page, settings.baseHref);
      fetchAppointments(page, settings.baseHref);
    }
  }, [props?.match?.params?.page, props?.params?.page, settings]);

  if (appointmentData.loaded && closeAppointmentData.loaded) {
    return (
      <div className="App App-home header-distance">
        <Modal isOpen={modal} toggle={toggle}>
          <ModalHeader toggle={toggle} className="beatabg">
            <div className="Aligner">
              <div className="Aligner-item Aligner-item--top"></div>
              <div className="Aligner-item">Citas en los próximos 2 días</div>
              <div className="Aligner-item Aligner-item--bottom"></div>
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
                    {closeAppointmentData.appointments.map(
                      (appointment: Appointment, index: number) => {
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
              </tr>
            </thead>
            <tbody>
              {appointmentData.appointments.map(
                (appointment: Appointment, index: number) => {
                  return (
                    <tr key={appointment.id}>
                      <th scope="row">{index}</th>
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
      </div>
    );
  }

  return <div className="App App-home header-distance">Loading...</div>;
};

export default AppointmentManager;
