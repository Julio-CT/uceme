import * as React from 'react';
import { render, screen } from '@testing-library/react';
import { unmountComponentAtNode } from 'react-dom';
import { MemoryRouter, Route } from 'react-router';
import Technique from './Technique';

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

describe('(Component)) Speciality', () => {
  it.skip('renders without exploding', () => {
    render(
      <MemoryRouter initialEntries={['especialidad/1']}>
        <Route path="especialidad/:esp">
          <Technique />
        </Route>
      </MemoryRouter>
    );
    expect(screen.queryAllByText('Loading', { exact: false })).toHaveLength(1);
  });

  it.skip('renders no buttons', () => {
    render(
      <MemoryRouter initialEntries={['especialidad/1']}>
        <Route path="especialidad/:esp">
          <Technique />
        </Route>
      </MemoryRouter>
    );
    expect(screen.queryAllByRole('button')).toHaveLength(0);
  });
});
