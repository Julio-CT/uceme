import * as React from 'react';
import { render, screen } from '@testing-library/react';
import { unmountComponentAtNode } from 'react-dom';
import AboutUs from './AboutUs';

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

describe('(Component)) AboutUs', () => {
  it('renders without exploding', () => {
    render(<AboutUs />, container);
    expect(
      screen.queryAllByText('Especialidades', { exact: false })
    ).toHaveLength(1);
  });

  it('renders no buttons', () => {
    render(<AboutUs />);
    expect(screen.queryAllByRole('button')).toHaveLength(0);
  });
});
