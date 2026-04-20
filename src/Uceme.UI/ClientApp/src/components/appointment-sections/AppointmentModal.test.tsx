import * as React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { unmountComponentAtNode } from 'react-dom';
import '@testing-library/jest-dom';
import AppointmentModal from './AppointmentModal';
// AppointmentHours intentionally not used in tests — removed import to satisfy linter
import SettingsContext from '../../SettingsContext';

let container: any;
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

const mockSettings = {
  baseHref: 'http://test.com/',
  telephone: '123456789',
  address: 'Test Address',
  contactEmail: 'test@test.com',
};

describe('(Component) AppointmentModal', () => {
  describe('handleValidation', () => {
    it('should have Confirmar disabled when required fields are empty', async () => {
      // Mock fetch sequence: hospitals -> days -> hours
      (global.fetch as jest.Mock) = jest
        .fn()
        .mockResolvedValueOnce({
          ok: true,
          json: async () => [{ idDatosPro: '1', nombre: 'Beata María Ana' }],
        })
        .mockResolvedValueOnce({
          ok: true,
          json: async () => [1, 2, 3, 4, 5],
        })
        .mockResolvedValueOnce({
          ok: true,
          json: async () => ({ hours: ['10:00'] }),
        });

      render(
        <SettingsContext.Provider value={mockSettings}>
          <AppointmentModal modal={true} toggle={() => {}} />
        </SettingsContext.Provider>
      );

      // Wait for hospital button to appear and click it
      const hospitalButton = await screen.findByText('Beata María Ana');
      fireEvent.click(hospitalButton);

      // Wait for the visible date input (placeholder) and set a date
      const dateInput = await screen.findByPlaceholderText('MM/DD/YYYY');
      fireEvent.change(dateInput, { target: { value: '01/01/2025' } });

      // Wait for hour button to appear and click it
      const hourButton = await screen.findByText('10:00');
      fireEvent.click(hourButton);

      // Now the form inputs should be visible; the Confirmar button should still be disabled
      const submitButtons = screen.getAllByRole('button', {
        name: 'Confirmar cita',
      });
      const disabledButton = submitButtons.find(
        (b) => (b as HTMLButtonElement).disabled
      );
      expect(disabledButton).toBeDefined();
    });

    it('should validate name field requirements', async () => {
      render(
        <SettingsContext.Provider value={mockSettings}>
          <AppointmentModal modal={true} toggle={() => {}} />
        </SettingsContext.Provider>
      );

      // Mock fetch sequence: hospitals -> days -> hours
      (global.fetch as jest.Mock) = jest
        .fn()
        .mockResolvedValueOnce({
          ok: true,
          json: async () => [{ idDatosPro: '1', nombre: 'Beata María Ana' }],
        })
        .mockResolvedValueOnce({
          ok: true,
          json: async () => [1, 2, 3, 4, 5],
        })
        .mockResolvedValueOnce({
          ok: true,
          json: async () => ({ hours: ['10:00'] }),
        });

      render(
        <SettingsContext.Provider value={mockSettings}>
          <AppointmentModal modal={true} toggle={() => {}} />
        </SettingsContext.Provider>
      );

      // Select hospital, choose date and hour to enable form
      const hospitalButton = await screen.findByText('Beata María Ana');
      fireEvent.click(hospitalButton);
      const dateInput = await screen.findByPlaceholderText('MM/DD/YYYY');
      fireEvent.change(dateInput, { target: { value: '01/01/2025' } });
      const hourButton = await screen.findByText('10:00');
      fireEvent.click(hourButton);

      // Test cases for name validation: to trigger submit we must set other required fields
      const nameInput = await screen.findByLabelText('Nombre completo');
      const phoneInput = await screen.findByLabelText('Teléfono');
      const emailInput = await screen.findByLabelText('Email de contacto');

      // Fill phone and email with valid values and check acceptTC to enable submit
      fireEvent.change(phoneInput, { target: { value: '+34123456789' } });
      fireEvent.change(emailInput, { target: { value: 'test@example.com' } });

      // Set name to too short and accept terms to enable button
      fireEvent.change(nameInput, { target: { value: 'Ana' } });
      const acceptCheckbox = await screen.findByRole('checkbox');
      fireEvent.click(acceptCheckbox);
      // Now submit should be enabled (find enabled Confirmar)
      const submitButtons = screen.getAllByRole('button', {
        name: 'Confirmar cita',
      });
      const enabledButton = submitButtons.find(
        (b) =>
          !(b as HTMLButtonElement).disabled &&
          (b as HTMLButtonElement).type === 'button'
      );
      expect(enabledButton).toBeDefined();
      fireEvent.click(enabledButton!);
      expect(
        await screen.findByText(
          'name: El nombre debe tener al menos 4 caracteres'
        )
      ).toBeInTheDocument();

      // Test space requirement
      fireEvent.change(nameInput, { target: { value: 'AnaGarcia' } });
      fireEvent.click(enabledButton!);
      expect(
        await screen.findByText('name: Por favor, introduce nombre y apellidos')
      ).toBeInTheDocument();

      // Test special characters
      fireEvent.change(nameInput, { target: { value: 'Ana Garcia123' } });
      fireEvent.click(enabledButton!);
      expect(
        await screen.findByText(
          'name: El nombre no puede contener caracteres especiales'
        )
      ).toBeInTheDocument();
    });

    it('should validate phone field requirements', async () => {
      // Mock fetch sequence: hospitals -> days -> hours
      (global.fetch as jest.Mock) = jest
        .fn()
        .mockResolvedValueOnce({
          ok: true,
          json: async () => [{ idDatosPro: '1', nombre: 'Beata María Ana' }],
        })
        .mockResolvedValueOnce({
          ok: true,
          json: async () => [1, 2, 3, 4, 5],
        })
        .mockResolvedValueOnce({
          ok: true,
          json: async () => ({ hours: ['10:00'] }),
        });

      render(
        <SettingsContext.Provider value={mockSettings}>
          <AppointmentModal modal={true} toggle={() => {}} />
        </SettingsContext.Provider>
      );

      // Select hospital, choose date and hour to enable form
      const hospitalButton = await screen.findByText('Beata María Ana');
      fireEvent.click(hospitalButton);
      const dateInput = await screen.findByPlaceholderText('MM/DD/YYYY');
      fireEvent.change(dateInput, { target: { value: '01/01/2025' } });
      const hourButton = await screen.findByText('10:00');
      fireEvent.click(hourButton);

      // Get inputs
      const phoneInput = await screen.findByLabelText('Teléfono');
      const nameInput = await screen.findByLabelText('Nombre completo');
      const emailInput = await screen.findByLabelText('Email de contacto');

      // Fill the other required fields so submit can run validation on phone
      fireEvent.change(nameInput, { target: { value: 'Ana Garcia' } });
      fireEvent.change(emailInput, { target: { value: 'test@example.com' } });
      // ensure phone is populated so the accept checkbox is rendered
      fireEvent.change(phoneInput, { target: { value: '000000000' } });
      const acceptCheckbox = await screen.findByRole('checkbox');
      fireEvent.click(acceptCheckbox);

      // Test invalid characters
      fireEvent.change(phoneInput, { target: { value: '123-456-789' } });
      const submitButtons = screen.getAllByRole('button', {
        name: 'Confirmar cita',
      });
      const enabledButton = submitButtons.find(
        (b) => !(b as HTMLButtonElement).disabled
      );
      fireEvent.click(enabledButton!);
      expect(
        await screen.findByText(
          'phone: El teléfono solo puede contener números, espacios y el símbolo "+"'
        )
      ).toBeInTheDocument();

      // Test valid phone number with spaces
      fireEvent.change(phoneInput, { target: { value: '+34 123 456 789' } });
      fireEvent.click(enabledButton!);
      await waitFor(() => {
        expect(
          screen.queryByText(
            'phone: El teléfono solo puede contener números, espacios y el símbolo "+"'
          )
        ).not.toBeInTheDocument();
      });

      // Test valid phone number without spaces
      fireEvent.change(phoneInput, { target: { value: '+34123456789' } });
      fireEvent.click(enabledButton!);
      await waitFor(() => {
        expect(
          screen.queryByText(
            'phone: El teléfono solo puede contener números, espacios y el símbolo "+"'
          )
        ).not.toBeInTheDocument();
      });
    });
  });
});
