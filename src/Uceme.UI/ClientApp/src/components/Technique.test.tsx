import * as React from 'react';
import { screen, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { MemoryRouter, Routes, Route } from 'react-router-dom';
import Technique from './Technique';
import {
  renderWithSettings,
  mockFetchSequence,
  clearFetchMock,
} from '../testUtils/testUtils';

const mockSettings = {
  baseHref: 'http://test.com/',
  telephone: '123456789',
  address: 'Test Address',
  contactEmail: 'test@test.com',
};

describe('(Component)) Technique', () => {
  afterEach(() => {
    clearFetchMock();
  });

  it('renders without exploding', async () => {
    const mockTechniqueResponse = {
      idTecnica: '3',
      titulo: 'Test Technique',
      texto: 'Test content',
      foto: '~/fotos/test.jpg',
      nombre: 'Test Name',
    };

    mockFetchSequence([
      { matcher: /tecnica/, response: mockTechniqueResponse, ok: true },
    ]);

    renderWithSettings(
      <MemoryRouter initialEntries={['/tecnica/3']}>
        <Routes>
          <Route path="/tecnica/:tec" element={<Technique />} />
        </Routes>
      </MemoryRouter>,
      mockSettings
    );

    // Initially shows loading
    expect(screen.getByText('Loading...')).toBeInTheDocument();

    // Wait for content to load
    await waitFor(() => {
      expect(screen.queryByText('Loading...')).not.toBeInTheDocument();
    });

    // Should render the technique content
    expect(screen.getByText('Test Technique')).toBeInTheDocument();
  });

  it('renders no buttons', async () => {
    const mockTechniqueResponse = {
      idTecnica: '3',
      titulo: 'Test Technique',
      texto: 'Test content',
      foto: '~/fotos/test.jpg',
      nombre: 'Test Name',
    };

    mockFetchSequence([
      { matcher: /tecnica/, response: mockTechniqueResponse, ok: true },
    ]);

    renderWithSettings(
      <MemoryRouter initialEntries={['/tecnica/3']}>
        <Routes>
          <Route path="/tecnica/:tec" element={<Technique />} />
        </Routes>
      </MemoryRouter>,
      mockSettings
    );

    // Wait for content to load
    await waitFor(() => {
      expect(screen.queryByText('Loading...')).not.toBeInTheDocument();
    });

    // Should not have any buttons
    expect(screen.queryAllByRole('button')).toHaveLength(0);
  });
});
