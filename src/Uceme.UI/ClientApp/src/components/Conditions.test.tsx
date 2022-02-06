import * as React from 'react';
import { render, screen } from '@testing-library/react';
import Conditions from './Conditions';

beforeAll(() => {});

afterAll(() => {});

describe('(Component)) Conditions', () => {
  it('renders without exploding', () => {
    render(<Conditions />);
    expect(screen.queryAllByText('informamos', { exact: false })).toHaveLength(
      1
    );
  });

  it('renders no buttons', () => {
    render(<Conditions />);
    expect(screen.queryAllByRole('button')).toHaveLength(0);
  });
});
