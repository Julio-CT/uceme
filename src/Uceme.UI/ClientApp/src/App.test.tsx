import * as React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';
import '@testing-library/jest-dom';
import { MemoryRouter } from 'react-router';
import { act } from 'react-dom/test-utils';

beforeAll(() => {});

afterAll(() => {});

describe('(Component)) App', () => {
  it('renders without exploding', () => {
    act(() => {
      render(
        <MemoryRouter>
          <App />
        </MemoryRouter>
      );
    });
    expect(screen.queryAllByText('Uceme', { exact: false })).toHaveLength(2);
  });
});
