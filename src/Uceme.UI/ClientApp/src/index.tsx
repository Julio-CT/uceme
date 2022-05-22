import 'bootstrap/dist/css/bootstrap.css';
import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import './index.scss';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');

if (baseUrl) {
  const container = document.getElementById('root');
  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  const root = createRoot(container!); // createRoot(container!) if you use TypeScript
  root.render(
    <BrowserRouter basename={baseUrl}>
      <App />
    </BrowserRouter>
  );
}
