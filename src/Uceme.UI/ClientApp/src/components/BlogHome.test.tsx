import * as React from 'react';
import { render, screen } from '@testing-library/react';
import { unmountComponentAtNode } from 'react-dom';
import { match, MemoryRouter, Route } from 'react-router';
import BlogHome from './BlogHome';

const path = `/route/:id`;

const match: match<{ id: string }> = {
  isExact: false,
  path,
  url: path.replace(':id', '1'),
  params: { id: '1' },
};

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

describe('(Component)) BlogHome', () => {
  it('renders without exploding', () => {
    render(
      <MemoryRouter initialEntries={['blogs/1']}>
        <Route path="blogs/:blogId">
          <BlogHome />
        </Route>
      </MemoryRouter>
    );
    expect(screen.queryAllByText('Loading..', { exact: false })).toHaveLength(
      1
    );
  });

  it('renders no buttons', () => {
    render(
      <MemoryRouter initialEntries={['blogs/1']}>
        <Route path="blogs/:blogId">
          <BlogHome />
        </Route>
      </MemoryRouter>
    );
    expect(screen.queryAllByRole('button')).toHaveLength(0);
  });
});
