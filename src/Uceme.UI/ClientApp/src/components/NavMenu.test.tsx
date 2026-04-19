import * as React from 'react';
import { render, screen } from '@testing-library/react';
import { unmountComponentAtNode } from 'react-dom';
import NavMenu from './NavMenu';

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

describe('(Component)) NavMenu', () => {
  it('renders without exploding and has 1 button', () => {
    render(<NavMenu />, container);
    expect(screen.queryAllByText('Uceme', { exact: false })).toHaveLength(1);
    expect(screen.queryAllByRole('button')).toHaveLength(1);
  });
});
