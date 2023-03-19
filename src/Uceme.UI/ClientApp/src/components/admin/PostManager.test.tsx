import * as React from 'react';
import { render, screen } from '@testing-library/react';
import PostManager from './PostManager';
import { unmountComponentAtNode } from 'react-dom';
import { act } from 'react-dom/test-utils';
import BlogPostResponse from '../../library/BlogPostResponse';

let container: any;
let history: any;
let location: any;
let match: any = { params: { page: 1 } };
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
  it('renders without exploding', async () => {
    const fakeBlog: BlogPostResponse[] = [
      {
        idBlog: Math.random().toString(),
        titulo: 'my girl',
        foto: 'oh',
        texto: 'my girl',
        fecha: 'talking about',
        slug: 'my',
        seoTitle: 'girl',
        metaDescription: 'mygirl',
        featuredImage: 'igotsunshine',
      },
    ];
    jest.spyOn(global, 'fetch').mockImplementation(() =>
      Promise.resolve({
        json: () => Promise.resolve(fakeBlog),
      } as Response)
    );

    await act(async () => {
      render(
        <PostManager history={history} location={location} match={match} />,
        container
      );
    });

    const headerText = screen.getAllByText('Loading...');
    expect(headerText).toBeTruthy();
  });
});
