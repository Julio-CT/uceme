import * as React from 'react';
import { screen, fireEvent, waitFor } from '@testing-library/react';
import AddPostModal from './AddPostModal';
import {
  renderWithSettings,
  mockFetchSequence,
  mockAuthToken,
  clearFetchMock,
} from '../../testUtils/testUtils';

describe('(Integration) AddPostModal', () => {
  const toggle = jest.fn();

  beforeEach(() => {
    jest.restoreAllMocks();
  });

  afterEach(() => {
    clearFetchMock();
  });

  it('uploads an image and publishes a post successfully', async () => {
    mockAuthToken('token');
    mockFetchSequence([
      {
        // ensure upload uses POST and includes FormData + Authorization header
        predicate: (url: string, opts?: any) =>
          String(url).includes('onpostuploadasync') &&
          opts &&
          String(opts.method).toUpperCase() === 'POST' &&
          !!(opts.body instanceof FormData) &&
          !!(opts.headers && opts.headers.Authorization),
        response: '/uploads/test.png',
        ok: true,
      },
      { matcher: /addpost/, method: 'POST', response: true, ok: true },
    ]);

    renderWithSettings(
      <AddPostModal toggle={toggle} modal={true} headerTitle={'Nuevo Post'} />
    );

    // fill required fields (title and slug share the same placeholder)
    const requiredInputs = screen.getAllByPlaceholderText('Campo requerido');
    fireEvent.change(requiredInputs[0], { target: { value: 'My Test Title' } });
    // second input is slug
    fireEvent.change(requiredInputs[1], { target: { value: 'my-test-title' } });

    fireEvent.change(
      screen.getByLabelText('Caption (descripción de la imagen):'),
      { target: { value: 'An image caption' } }
    );
    fireEvent.change(screen.getByLabelText('Meta-description:'), {
      target: { value: 'meta' },
    });
    fireEvent.change(screen.getByLabelText('Título para SEO:'), {
      target: { value: 'seo title' },
    });

    // simulate selecting a file
    const file = new File(['dummy'], 'photo.png', { type: 'image/png' });
    const fileInput =
      screen.getByLabelText('Imagen (max 600x600px):').closest('input') ||
      screen.getByLabelText('Imagen (max 600x600px):');
    // fire change with files
    // @ts-ignore
    fireEvent.change(fileInput, { target: { files: [file] } });

    // click upload button
    const uploadButton = screen.getByText('Subir imagen');
    fireEvent.click(uploadButton);

    // wait for upload success alert
    await waitFor(() =>
      expect(screen.getByText('Imagen subida correctamente.')).toBeTruthy()
    );

    // close alert
    fireEvent.click(screen.getByText('Cerrar'));

    // click publish
    const publish = screen.getByText('Publicar');
    fireEvent.click(publish);

    // wait for post registered alert
    await waitFor(() =>
      expect(
        screen.getByText('Post registrado correctamente. Muchas gracias.')
      ).toBeTruthy()
    );
  });
});
