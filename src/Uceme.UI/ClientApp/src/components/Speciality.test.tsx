import * as React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import Speciality from './Speciality';
import { MemoryRouter, Routes, Route } from 'react-router-dom';
import SettingsContext from '../SettingsContext';

const mockSettings = {
  baseHref: 'http://test.com/',
  telephone: '123456789',
  address: 'Test Address',
  contactEmail: 'test@test.com',
};

describe('(Component)) Speciality', () => {
  it('renders without exploding', async () => {
    render(
      <SettingsContext.Provider value={mockSettings}>
        <MemoryRouter initialEntries={['/especialidad/cirugia']}>
          <Routes>
            <Route path="/especialidad/:esp" element={<Speciality />} />
          </Routes>
        </MemoryRouter>
      </SettingsContext.Provider>
    );

    // Initially shows loading
    expect(screen.getByText('Loading...')).toBeInTheDocument();

    // Wait for content to load
    await waitFor(() => {
      expect(screen.queryByText('Loading...')).not.toBeInTheDocument();
    });

    // Should render the speciality content
    expect(screen.getByText('Cirugía Tiroidea')).toBeInTheDocument();
  });

  it('renders no buttons', async () => {
    render(
      <SettingsContext.Provider value={mockSettings}>
        <MemoryRouter initialEntries={['/especialidad/cirugia']}>
          <Routes>
            <Route path="/especialidad/:esp" element={<Speciality />} />
          </Routes>
        </MemoryRouter>
      </SettingsContext.Provider>
    );

    // Wait for content to load
    await waitFor(() => {
      expect(screen.queryByText('Loading...')).not.toBeInTheDocument();
    });

    // Should not have any buttons
    expect(screen.queryAllByRole('button')).toHaveLength(0);
  });
});
