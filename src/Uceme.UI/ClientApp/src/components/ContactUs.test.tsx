import * as React from 'react';
import { render, screen } from '@testing-library/react';
import { unmountComponentAtNode } from 'react-dom';
import ContactUs from './ContactUs';
import { MemoryRouter, Route } from 'react-router';

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

describe('(Component)) ContactUs', () => {
  it('renders without exploding', () => {
    render(
      <MemoryRouter initialEntries={['contacto']}>
        <Route path='contacto'>
          <ContactUs />
        </Route>
      </MemoryRouter>
    );
    expect(screen.queryAllByText('Horario de atención', { exact: false })).toHaveLength(
      1
    );
  });

  it('renders 1 buttons', () => {
    render(
      <MemoryRouter initialEntries={['contacto']}>
        <Route path='contacto'>
          <ContactUs />
        </Route>
      </MemoryRouter>
    );
    expect(screen.queryAllByRole('button')).toHaveLength(1);
  });
});
