import * as React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { unmountComponentAtNode } from 'react-dom';
import '@testing-library/jest-dom';
import AppointmentModal from './AppointmentModal';
import AppointmentHours from './AppointmentHours';
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
    it('should show validation errors for empty required fields', () => {
      render(
        <SettingsContext.Provider value={mockSettings}>
          <AppointmentModal modal={true} toggle={() => {}} />
        </SettingsContext.Provider>
      );

      // Find and click submit button to trigger validation
      const submitButton = screen.getByText('Confirmar cita');
      fireEvent.click(submitButton);

      // Check for required field error messages
      expect(
        screen.getByText('day: Este campo es obligatorio')
      ).toBeInTheDocument();
      expect(
        screen.getByText('hour: Este campo es obligatorio')
      ).toBeInTheDocument();
      expect(
        screen.getByText('email: Este campo es obligatorio')
      ).toBeInTheDocument();
      expect(
        screen.getByText('name: Este campo es obligatorio')
      ).toBeInTheDocument();
      expect(
        screen.getByText('phone: Este campo es obligatorio')
      ).toBeInTheDocument();
      expect(
        screen.getByText('acceptTC: Este campo es obligatorio')
      ).toBeInTheDocument();
    });

    it('should validate name field requirements', async () => {
      render(
        <SettingsContext.Provider value={mockSettings}>
          <AppointmentModal modal={true} toggle={() => {}} />
        </SettingsContext.Provider>
      );

      // Enable form by simulating hour selection
      const mockHour = '10:00';
      const mockHourSelect = jest.fn();
      render(
        <AppointmentHours hours={[mockHour]} onSelectedHour={mockHourSelect} />
      );
      fireEvent.click(screen.getByText(mockHour));

      // Test cases for name validation
      const nameInput = screen.getByPlaceholderText('Campo requerido');

      // Test minimum length
      fireEvent.change(nameInput, { target: { value: 'Ana' } });
      fireEvent.click(screen.getByText('Confirmar cita'));
      expect(
        screen.getByText('name: El nombre debe tener al menos 4 caracteres')
      ).toBeInTheDocument();

      // Test space requirement
      fireEvent.change(nameInput, { target: { value: 'AnaGarcia' } });
      fireEvent.click(screen.getByText('Confirmar cita'));
      expect(
        screen.getByText('name: Por favor, introduce nombre y apellidos')
      ).toBeInTheDocument();

      // Test special characters
      fireEvent.change(nameInput, { target: { value: 'Ana Garcia123' } });
      fireEvent.click(screen.getByText('Confirmar cita'));
      expect(
        screen.getByText(
          'name: El nombre no puede contener caracteres especiales'
        )
      ).toBeInTheDocument();
    });

    it('should validate phone field requirements', async () => {
      render(
        <SettingsContext.Provider value={mockSettings}>
          <AppointmentModal modal={true} toggle={() => {}} />
        </SettingsContext.Provider>
      );

      // Enable form by simulating hour selection
      const mockHour = '10:00';
      const mockHourSelect = jest.fn();
      render(
        <AppointmentHours hours={[mockHour]} onSelectedHour={mockHourSelect} />
      );
      fireEvent.click(screen.getByText(mockHour));

      // Test cases for phone validation
      const phoneInput = screen.getByPlaceholderText('Campo requerido');

      // Test invalid characters
      fireEvent.change(phoneInput, { target: { value: '123-456-789' } });
      fireEvent.click(screen.getByText('Confirmar cita'));
      expect(
        screen.getByText(
          'phone: El teléfono solo puede contener números, espacios y el símbolo "+"'
        )
      ).toBeInTheDocument();

      // Test valid phone number with spaces
      fireEvent.change(phoneInput, { target: { value: '+34 123 456 789' } });
      fireEvent.click(screen.getByText('Confirmar cita'));
      expect(
        screen.queryByText(
          'phone: El teléfono solo puede contener números, espacios y el símbolo "+"'
        )
      ).not.toBeInTheDocument();

      // Test valid phone number without spaces
      fireEvent.change(phoneInput, { target: { value: '+34123456789' } });
      fireEvent.click(screen.getByText('Confirmar cita'));
      expect(
        screen.queryByText(
          'phone: El teléfono solo puede contener números, espacios y el símbolo "+"'
        )
      ).not.toBeInTheDocument();
    });
  });
});
