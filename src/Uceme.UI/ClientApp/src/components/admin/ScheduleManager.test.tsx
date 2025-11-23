import * as React from 'react';
import { screen, waitFor, fireEvent } from '@testing-library/react';
import ScheduleManager from './ScheduleManager';
import {
  renderWithSettings,
  mockFetchSequence,
  mockAuthToken,
  clearFetchMock,
} from '../../testUtils/testUtils';

describe('(Integration) ScheduleManager', () => {
  beforeEach(() => jest.restoreAllMocks());
  afterEach(() => clearFetchMock());

  it('renders hospitals and turns from API and opens add hospital modal', async () => {
    mockAuthToken('token');

    const fakeHospitals = [
      { idDatosPro: 1, nombre: 'Hospital Uno', direccion: 'Calle 1' },
    ];

    const fakeTurns = [
      {
        idTurno: 1,
        idHospital: 1,
        dia: '1',
        diaSemana: 'Lunes',
        inicio: '08:00',
        fin: '12:00',
        paralelas: 2,
        porhora: 4,
      },
    ];

    // mock fetch: turns then hospitals (explicit GET)
    mockFetchSequence([
      { matcher: /turns/, method: 'GET', response: fakeTurns, ok: true },
      {
        matcher: /hospitals/,
        method: 'GET',
        response: fakeHospitals,
        ok: true,
      },
    ]);

    renderWithSettings(<ScheduleManager />);

    // wait for the hospital to appear in the table (may appear multiple times)
    await waitFor(() =>
      expect(screen.getAllByText('Hospital Uno').length).toBeGreaterThan(0)
    );

    // check turn info present
    expect(screen.getByText('08:00')).toBeTruthy();

    // click 'Añadir Hospital' to open add hospital modal
    fireEvent.click(screen.getByText(/Añadir Hospital/));
    // modal title appears as a heading
    await waitFor(() =>
      expect(
        screen.getByRole('heading', { name: 'Añadir Hospital' })
      ).toBeTruthy()
    );
  });
});
