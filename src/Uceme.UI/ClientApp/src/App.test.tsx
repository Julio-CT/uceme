import * as React from 'react';
import { render } from '@testing-library/react';
import ReactDOM from 'react-dom';
import { MemoryRouter } from 'react-router-dom';
import App from './App';
import '@testing-library/jest-dom';

beforeAll(() => {});

afterAll(() => {});

test('renders without crashing', async () => {
  const div = document.createElement('div');
  ReactDOM.render(
    <MemoryRouter>
      <App />
    </MemoryRouter>,
    div
  );
  await new Promise((resolve) => setTimeout(resolve, 1000));
});

test('renders somos uceme link', () => {
  const { getByText } = render(<App />);
  const linkElement = getByText(/Somos Uceme/i);
  expect(linkElement).toBeInTheDocument();
});
