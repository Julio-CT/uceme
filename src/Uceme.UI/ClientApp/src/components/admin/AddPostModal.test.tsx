import * as React from 'react';
import { render, screen } from '@testing-library/react';
import AddPostModal from './AddPostModal';
import { unmountComponentAtNode } from 'react-dom';

let container: any;
const editToggle = () => {};
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

describe('(Component)) AddPostModal', () => {
  it('renders without exploding', () => {
    render(<AddPostModal toggle={editToggle} modal={true} headerTitle={"hello"} />, container);
    const image = screen.getAllByRole('button');
    expect(image).toBeTruthy();
  });

  it('renders all buttons', () => {
    render(<AddPostModal toggle={editToggle} modal={true} headerTitle={"world"} />, container);
    const image = screen.getAllByRole('button');
    expect(image).toBeTruthy();
  });
});
