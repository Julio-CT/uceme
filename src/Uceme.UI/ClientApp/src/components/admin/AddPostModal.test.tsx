import * as React from 'react';
import { act, render, screen } from '@testing-library/react';
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
    act(() => {
      render(
        <AddPostModal toggle={editToggle} modal={true} headerTitle={'hello'} />,
        container
      );
    });
    const buttons = screen.getAllByRole('button');
    expect(buttons).toBeTruthy();
  });

  it('renders all buttons', () => {
    act(() => {
      render(
        <AddPostModal toggle={editToggle} modal={true} headerTitle={'world'} />,
        container
      );
    });
    const buttons = screen.getAllByRole('button');
    expect(buttons.length).toBe(4);
  });
});
