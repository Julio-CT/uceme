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
  it('renders without exploding', () => {
    render(<NavMenu />, container);
    expect(screen.queryAllByText('Uceme', { exact: false })).toHaveLength(1);
  });

  it('renders 1 buttons', () => {
    render(<NavMenu />);
    expect(screen.queryAllByRole('button')).toHaveLength(1);
  });
});
