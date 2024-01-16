import React, { ReactElement } from 'react';
import moment from 'moment';
import { Calendar, Views, momentLocalizer, Event } from 'react-big-calendar';
import './AppointmentManager.scss';
import './ScheduleManager.scss';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button } from 'reactstrap';
import * as dates from '../../utils/dates';
import 'react-big-calendar/lib/css/react-big-calendar.css';
import SettingsContext, { Settings } from '../../SettingsContext';
import Appointment from '../../library/Appointment';
import AppointmentResponse from '../../library/AppointmentResponse';
import { DateTimeUtils } from '../../library/DateTimeUtils';
import authService from '../api-authorization/AuthorizeService';
import EventResponse from '../../library/EventResponse';

type ScheduleManagerState = {
  loaded: boolean;
  events?: ExtendedEvent[] | undefined;
  backgrondEvents?: ExtendedEvent[] | null;
};

type AppointmentManagerState = {
  loaded: boolean;
  appointments?: Appointment[] | null;
};

interface ExtendedEvent extends Event {
  id: number;
  desc?: string | undefined;
}

function ScheduleManager(): ReactElement {
  const settings: Settings = React.useContext(SettingsContext);
  const [closeAppointmentsModal, setCloseAppointmentsModal] =
    React.useState<boolean>(false);
  const closeAppointmentsToggle = () =>
    setCloseAppointmentsModal(!closeAppointmentsModal);
  const localizer = momentLocalizer(moment);
  const [appointmentData, setAppointmentData] =
    React.useState<ScheduleManagerState>({
      loaded: false,
      events: undefined,
      backgrondEvents: null,
    });
  const [closeAppointmentData, setCloseAppointmentData] =
    React.useState<AppointmentManagerState>({
      loaded: false,
      appointments: null,
    });

  const isFirstRun = React.useRef(true);

  const { defaultDate, max, views } = React.useMemo(
    () => ({
      defaultDate: new Date(2015, 3, 13),
      max: dates.add(dates.endOf(new Date(2015, 17, 1), 'day'), -1, 'hours'),
      views: [Views.WEEK, Views.DAY, Views.AGENDA],
    }),
    []
  );

  const backgroundEvents: ExtendedEvent[] = [
    {
      id: 0,
      title: 'Available for Clients',
      start: new Date(2015, 3, 13, 6),
      end: new Date(2015, 3, 13, 18),
    },
  ];

  React.useEffect(() => {
    const fetchAppointments = async () => {
      const token = await authService.getAccessToken();
      if (settings?.baseHref !== undefined) {
        fetch(`${settings?.baseHref}api/appointment/AppointmentEventsList`, {
          headers: !token ? {} : { Authorization: `Bearer ${token}` },
        })
          .then((response: { json: () => Promise<EventResponse[]> }) =>
            response.json()
          )
          .then(async (resp: EventResponse[]) => {
            const retrievedAppointments: ExtendedEvent[] = [];

            await Promise.all(
              resp.map(async (obj: EventResponse) => {
                retrievedAppointments.push({
                  id: obj.id,
                  title: obj.title,
                  desc: obj.desc,
                  start: DateTimeUtils.DateFromEvent(obj.start),
                  end: DateTimeUtils.DateFromEvent(obj.end),
                });
              })
            );

            setAppointmentData({
              loaded: true,
              events: retrievedAppointments,
              backgrondEvents: null,
            });
          })
          .catch(() => {
            setAppointmentData({
              loaded: false,
              events: undefined,
              backgrondEvents: null,
            });
          });
      }
    };

    const fetchCloseAppointments = async () => {
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
              events: undefined,
              backgrondEvents: null,
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

      fetchCloseAppointments();
      fetchAppointments();
      updatePastAppointmentsData();
      return;
    }

    setAppointmentData({ loaded: false });
    fetchCloseAppointments();
    fetchAppointments();
  }, [settings?.baseHref]);

  if (appointmentData.loaded && closeAppointmentData.loaded) {
    return (
      <div className="app app-home header-distance">
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
        <Calendar
          backgroundEvents={backgroundEvents}
          dayLayoutAlgorithm="no-overlap"
          defaultDate={defaultDate}
          defaultView={Views.DAY}
          events={appointmentData.events}
          localizer={localizer}
          max={max}
          showMultiDayTimes
          step={60}
          views={views}
        />
      </div>
    );
  }

  return <div className="app app-home header-distance">Loading...</div>;
}

export default ScheduleManager;
