import * as React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import LoginMenu from './LoginMenu';
import authService from './AuthorizeService';

describe('LoginMenu integration', () => {
  beforeEach(() => {
    jest.restoreAllMocks();
  });

  it('renders nothing when user is not authenticated', async () => {
    // Arrange: ensure authService methods resolve to unauthenticated
    (authService.isAuthenticated as unknown) = jest
      .fn()
      .mockResolvedValue(false);
    (authService.getUser as unknown) = jest.fn().mockResolvedValue(null);

    render(<LoginMenu />);

    // Should not show protected links
    expect(screen.queryByText('Citas')).not.toBeInTheDocument();
    expect(screen.queryByText('Posts')).not.toBeInTheDocument();
  });

  it('renders authenticated links after authService updates state', async () => {
    // Arrange: stub the async authService calls to return authenticated user
    (authService.isAuthenticated as unknown) = jest
      .fn()
      .mockResolvedValue(true);
    (authService.getUser as unknown) = jest
      .fn()
      .mockResolvedValue({ name: 'juan@example.com' });

    render(<LoginMenu />);

    // Act: trigger notify by updating the auth service state (calls subscribers)
    authService.updateState({} as any);

    // Assert: wait for protected links to appear
    await waitFor(() => expect(screen.getByText('Citas')).toBeInTheDocument());
    expect(screen.getByText('Posts')).toBeInTheDocument();
    expect(screen.getByText('Horarios')).toBeInTheDocument();
    expect(screen.getByText('Salir')).toBeInTheDocument();
  });
});

// no exports from test file
