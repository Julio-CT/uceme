import 'bootstrap/dist/css/bootstrap.css';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import './index.scss';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');

if (baseUrl) {
  ReactDOM.render(
    <BrowserRouter basename={baseUrl}>
      <App />
    </BrowserRouter>,
    document.getElementById('root')
  );
}
