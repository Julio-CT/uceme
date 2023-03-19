import * as React from 'react';
import { render, screen } from '@testing-library/react';
import AppointmentManager from './AppointmentManager';
import { unmountComponentAtNode } from 'react-dom';
import { act } from 'react-dom/test-utils';
import AppointmentResponse from '../../library/AppointmentResponse';

let container: any;
let history: any;
let location: any;
let match: any = { params: { page: 1 } };
beforeEach(() => {
  // setup a DOM element as a render target
  container = document.createElement('div');
  document.body.appendChild(container);
});

afterEach(() => {
  // cleanup on exiting
  unmountComponentAtNode(container);
  container.remove();
  container = null;
});

describe('(Component)) AddPostModal', () => {
  it('renders without exploding', async () => {
    const fakeAppointment: AppointmentResponse[] = [
      {
        idCita: Math.random().toString(),
        dia: 'lunes',
        hora: '32',
        nombre: 'Johny',
        email: '',
        telefono: '',
        idTurno: '',
        speciality: '',
      },
    ];
    jest.spyOn(global, 'fetch').mockImplementation(() =>
      Promise.resolve({
        json: () => Promise.resolve(fakeAppointment),
      } as Response)
    );

    await act(async () => {
      render(
        <AppointmentManager
          history={history}
          location={location}
          match={match}
        />,
        container
      );
    });

    const headerText = screen.getAllByText('Citas en los próximos 2 días');
    expect(headerText).toBeTruthy();
  });

  it('renders all the buttons', async () => {
    const fakeAppointment: AppointmentResponse[] = [
      {
        idCita: Math.random().toString(),
        dia: 'lunes',
        hora: '32',
        nombre: 'Johny',
        email: '',
        telefono: '',
        idTurno: '',
        speciality: '',
      },
    ];
    jest.spyOn(global, 'fetch').mockImplementation(() =>
      Promise.resolve({
        json: () => Promise.resolve(fakeAppointment),
      } as Response)
    );

    await act(async () => {
      render(
        <AppointmentManager
          history={history}
          location={location}
          match={match}
        />,
        container
      );
    });

    const buttons = screen.getAllByRole('button');
    expect(buttons.length).toBe(2);
  });
});
