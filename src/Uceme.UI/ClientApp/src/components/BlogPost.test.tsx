import * as React from 'react';
import { render, screen } from '@testing-library/react';
import { unmountComponentAtNode } from 'react-dom';
import BlogPost from './BlogPost';
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

describe('(Component)) BlogPost', () => {
  it('renders without exploding', () => {
    render(
      <MemoryRouter initialEntries={['post/1']}>
        <Route path='post/:slug'>
          <BlogPost />
        </Route>
      </MemoryRouter>
    );
    expect(screen.queryAllByText('Loading', { exact: false })).toHaveLength(
      1
    );
  });

  it('renders no buttons', () => {
    render(
      <MemoryRouter initialEntries={['post/1']}>
        <Route path='post/:slug'>
          <BlogPost />
        </Route>
      </MemoryRouter>
    );
    expect(screen.queryAllByRole('button')).toHaveLength(0);
  });
});
