import * as React from 'react';
import { render, RenderOptions, screen } from '@testing-library/react';
import Conditions from './Conditions';
import { unmountComponentAtNode } from 'react-dom';

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

describe('(Component)) Conditions', () => {
  it('renders without exploding', () => {
    render(<Conditions />, container);
    expect(screen.queryAllByText('informamos', { exact: false })).toHaveLength(
      1
    );
  });

  it('renders no buttons', () => {
    render(<Conditions />);
    expect(screen.queryAllByRole('button')).toHaveLength(0);
  });
});
