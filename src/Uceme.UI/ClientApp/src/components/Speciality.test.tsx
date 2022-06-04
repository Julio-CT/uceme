import * as React from 'react';
import { render, screen } from '@testing-library/react';
import { unmountComponentAtNode } from 'react-dom';
import Speciality from './Speciality';
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

describe('(Component)) Speciality', () => {
  it('renders without exploding', () => {
    render(
      <MemoryRouter initialEntries={['especialidad/1']}>
        <Route path='especialidad/:esp'>
          <Speciality />
        </Route>
      </MemoryRouter>
    );
    expect(screen.queryAllByText('Loading', { exact: false })).toHaveLength(
      1
    );
  });

  it('renders no buttons', () => {
    render(
      <MemoryRouter initialEntries={['especialidad/1']}>
        <Route path='especialidad/:esp'>
          <Speciality />
        </Route>
      </MemoryRouter>
    );
    expect(screen.queryAllByRole('button')).toHaveLength(0);
  });
});
