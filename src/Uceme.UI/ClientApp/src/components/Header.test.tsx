import * as React from 'react';
import { render, screen } from '@testing-library/react';
import { unmountComponentAtNode } from 'react-dom';
import Header from './Header';

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

describe('(Component)) Header', () => {
  it('renders without exploding', () => {
    render(<Header />, container);
    expect(
      screen.queryAllByText(
        'Unidad de Cirugía Endocrinometabólica Especializada',
        { exact: false }
      )
    ).toHaveLength(1);
  });

  it('renders 2 buttons', () => {
    render(<Header />);
    expect(screen.queryAllByRole('button')).toHaveLength(2);
  });
});
