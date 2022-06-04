import * as React from 'react';
import { render, screen } from '@testing-library/react';
import { unmountComponentAtNode } from 'react-dom';
import Footer from './Footer';

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

describe('(Component)) Footer', () => {
  it('renders without exploding', () => {
    render(<Footer />, container);
    expect(screen.queryAllByText('facebook', { exact: false })).toHaveLength(
      1
    );
  });

  it('renders no buttons', () => {
    render(<Footer />);
    expect(screen.queryAllByRole('button')).toHaveLength(0);
  });
});
