import * as React from 'react';
import { screen, waitFor, fireEvent, within } from '@testing-library/react';
import AppointmentManager from './AppointmentManager';
import {
  renderWithSettings,
  mockFetchSequence,
  mockAuthToken,
  clearFetchMock,
} from '../../testUtils/testUtils';

describe('(Integration) AppointmentManager', () => {
  afterEach(() => {
    clearFetchMock();
    jest.restoreAllMocks();
  });

  it('renders appointments and upcoming close appointments modal', async () => {
    mockAuthToken('token');

    const appointmentApiResp = [
      {
        idCita: 10,
        speciality: 'Cardiology',
        dia: '2025-11-23T00:00:00.000Z',
        hora: '08:00:00',
        nombre: 'Alice',
        email: 'alice@test.com',
        telefono: '111-222',
        idTurno: 1,
      },
    ];

    const closeResp = [
      {
        idCita: 11,
        speciality: 'Dermatology',
        dia: '2025-11-24T00:00:00.000Z',
        hora: '09:30:00',
        nombre: 'Bob',
        email: 'bob@test.com',
        telefono: '333-444',
        idTurno: 2,
      },
    ];

    mockFetchSequence([
      {
        matcher: /appointment\/appointmentlist$/,
        method: 'GET',
        response: appointmentApiResp,
        ok: true,
      },
      {
        matcher: /closeappointmentlist$/,
        method: 'GET',
        response: closeResp,
        ok: true,
      },
      { matcher: /past-appointments/, method: 'PUT', response: true, ok: true },
    ]);

    renderWithSettings(<AppointmentManager />);

    // wait for appointment row to appear (may appear in both table and modal)
    await waitFor(() =>
      expect(screen.getAllByText('Alice').length).toBeGreaterThan(0)
    );

    // The close appointments modal header should be present indicating nearby appointments
    await waitFor(() =>
      expect(screen.getByText('Citas en los próximos 2 días')).toBeTruthy()
    );

    // close appointment's patient should appear in the close-appointments modal
    const heading = screen.getByText(/Citas en los próximos 2 días/);
    const closeModal = heading.closest('[role="dialog"]') as HTMLElement;
    expect(within(closeModal).getByText('Bob')).toBeTruthy();
  });

  it('deletes an appointment and shows success alert', async () => {
    mockAuthToken('token');

    const appointmentApiResp = [
      {
        idCita: 20,
        speciality: 'ENT',
        dia: '2025-11-23T00:00:00.000Z',
        hora: '10:00:00',
        nombre: 'Carlos',
        email: 'carlos@test.com',
        telefono: '555-666',
        idTurno: 3,
      },
    ];

    mockFetchSequence([
      {
        matcher: /appointment\/appointmentlist$/,
        method: 'GET',
        response: appointmentApiResp,
        ok: true,
      },
      {
        matcher: /closeappointmentlist$/,
        method: 'GET',
        response: [],
        ok: true,
      },
      // delete predicate: ensure method DELETE and Authorization header present
      {
        predicate: (url: string, opts?: any) =>
          /api\/appointment\/.+/.test(url) &&
          opts &&
          String(opts.method).toUpperCase() === 'DELETE' &&
          !!(opts.headers && opts.headers.Authorization),
        response: true,
        ok: true,
      },
      { matcher: /past-appointments/, method: 'PUT', response: true, ok: true },
    ]);

    renderWithSettings(<AppointmentManager />);

    // wait for appointment row (may appear multiple times)
    await waitFor(() =>
      expect(screen.getAllByText('Carlos').length).toBeGreaterThan(0)
    );

    // click delete icon (aria-label="Delete")
    const deleteIcon = screen.getAllByLabelText('Delete')[0];
    fireEvent.click(deleteIcon);

    // confirmation modal should show
    const confirmText = await screen.findByText(
      /¿Está seguro de eliminar la cita/
    );
    const confirmDialog = confirmText.closest('[role="dialog"]') as HTMLElement;

    // click 'Eliminar' button within confirmation dialog to avoid ambiguous matches
    fireEvent.click(
      within(confirmDialog).getByRole('button', { name: 'Eliminar' })
    );

    // wait for alert message
    await waitFor(() =>
      expect(
        screen.getByText('Cita previa borrada correctamente. Muchas gracias.')
      ).toBeTruthy()
    );
  });

  it('shows no appointments modal when API returns 204', async () => {
    mockAuthToken('token');

    // Use empty array response to trigger the "no appointments" modal reliably
    mockFetchSequence([
      {
        matcher: /appointment\/appointmentlist$/,
        method: 'GET',
        response: [],
        ok: true,
      },
      {
        matcher: /closeappointmentlist$/,
        method: 'GET',
        response: [],
        ok: true,
      },
      { matcher: /past-appointments/, method: 'PUT', response: true, ok: true },
    ]);

    renderWithSettings(<AppointmentManager />);

    // wait for no appointments modal content
    await waitFor(() =>
      expect(
        screen.getByText('No hay citas desde los ultimos 30 dias para mostrar.')
      ).toBeTruthy()
    );
  });
});
