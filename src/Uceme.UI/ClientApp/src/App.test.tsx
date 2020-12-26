import * as React from 'react';
import { render } from '@testing-library/react';
import App from './App';
import ReactDOM from 'react-dom';
import { MemoryRouter } from 'react-router-dom';
import '@testing-library/jest-dom/extend-expect';

it('renders without crashing', async () => {
    const div = document.createElement('div');
    ReactDOM.render(
        <MemoryRouter>
            <App />
        </MemoryRouter>, div);
    await new Promise(resolve => setTimeout(resolve, 1000));
});

test('renders learn react link', () => {
    const { getByText } = render(<App />);
    const linkElement = getByText(/learn react/i);
    expect(linkElement).toBeInTheDocument();
});
