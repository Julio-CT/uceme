import * as React from 'react';
import { render, screen } from '@testing-library/react';
import { unmountComponentAtNode } from 'react-dom';
import Specialities from './Specialities';

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

describe('(Component)) Specialities', () => {
  it('renders without exploding and has no buttons', () => {
    render(<Specialities />, container);
    expect(
      screen.queryAllByText('Especialidades', { exact: false })
    ).toHaveLength(1);
    expect(screen.queryAllByRole('button')).toHaveLength(0);
  });
});
