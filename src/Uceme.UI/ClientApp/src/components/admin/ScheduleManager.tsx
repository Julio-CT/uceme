import React, { ReactElement } from 'react';
import { useParams } from 'react-router-dom';
import Appointment from '../../library/Appointment';
import AppointmentResponse from '../../library/AppointmentResponse';
import { DateTimeUtils } from '../../library/DateTimeUtils';
import './AppointmentManager.scss';
import '../appointment-sections/AppointmentModal.scss';
import authService from '../api-authorization/AuthorizeService';
import SettingsContext, { Settings } from '../../SettingsContext';

type ScheduleManagerState = {
  loaded: boolean;
  appointments?: Appointment[] | null;
  page?: number;
};

function ScheduleManager(): ReactElement {
  const { page } = useParams();
  const settings: Settings = React.useContext(SettingsContext);
  const [appointmentData, setAppointmentData] =
    React.useState<ScheduleManagerState>({
      loaded: false,
      appointments: null,
      page: page ? +page : 1,
    });
  const [closeAppointmentData, setCloseAppointmentData] =
    React.useState<ScheduleManagerState>({
      loaded: false,
      appointments: null,
      page: page ? +page : 1,
    });

  const isFirstRun = React.useRef(true);

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
      <div className="app app-home header-distance-l">Work in progress...</div>
    );
  }

  return <div className="app app-home header-distance">Loading...</div>;
}

export default ScheduleManager;
