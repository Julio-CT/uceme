import * as React from 'react';
import { screen, waitFor, fireEvent } from '@testing-library/react';
import PostManager from './PostManager';
import {
  renderWithSettings,
  mockFetchSequence,
  mockAuthToken,
  clearFetchMock,
} from '../../testUtils/testUtils';

describe('(Integration) PostManager', () => {
  beforeEach(() => jest.restoreAllMocks());
  afterEach(() => clearFetchMock());

  it('fetches posts and allows deleting a post (confirmation flow)', async () => {
    mockAuthToken('token');

    const fakePosts = [
      {
        idBlog: 1,
        titulo: 'Test Post',
        foto: '/uploads/test.png',
        texto: '<p>hello</p>',
        metaDescription: 'meta',
        seoTitle: 'seo',
        slug: 'test-post',
        fecha: '2020-01-01T00:00:00.000Z',
      },
    ];

    mockFetchSequence([
      { matcher: /getallposts/, method: 'GET', response: fakePosts, ok: true },
      {
        // ensure delete call is DELETE and includes Authorization header
        predicate: (url: string, opts?: any) =>
          /deletepost/.test(url) &&
          opts &&
          String(opts.method).toUpperCase() === 'DELETE' &&
          !!(opts.headers && opts.headers.Authorization),
        response: true,
        ok: true,
      },
      { matcher: /getallposts/, method: 'GET', response: [], ok: true },
    ]);

    renderWithSettings(<PostManager />);

    // wait for the post title to appear
    await waitFor(() => expect(screen.getByText('Test Post')).toBeTruthy());

    // click delete icon (aria-label="Delete")
    const deleteButton = screen.getByLabelText('Delete');
    fireEvent.click(deleteButton);

    // confirm modal should appear with the title
    await waitFor(() =>
      expect(screen.getByText(/¿Está seguro de borrar el post/)).toBeTruthy()
    );

    // click the Borrar button in confirmation (use role to avoid header text)
    const confirm = screen.getByRole('button', { name: 'Borrar' });
    fireEvent.click(confirm);

    // wait for the alert message about deletion
    await waitFor(() =>
      expect(
        screen.getByText('Post borrado correctamente. Muchas gracias.')
      ).toBeTruthy()
    );
  });

  it('opens AddPost modal when clicking Añadir Post', async () => {
    mockAuthToken('token');
    mockFetchSequence([
      { matcher: /getallposts/, method: 'GET', response: [], ok: true },
    ]);
    renderWithSettings(<PostManager />);

    // wait for loading to finish
    await waitFor(() => expect(screen.getByText('Añadir Post')).toBeTruthy());

    fireEvent.click(screen.getByText('Añadir Post'));

    // AddPostModal should be present with header 'Nuevo Post'
    await waitFor(() => expect(screen.getByText('Nuevo Post')).toBeTruthy());
  });
});
