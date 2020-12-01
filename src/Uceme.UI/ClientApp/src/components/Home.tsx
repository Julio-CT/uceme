import * as React from 'react';
import { Component } from 'react';
import logo, { ReactComponent as Logo } from '../resources/images/logo.svg';

import './Home.scss';
import Slider from './home-sections/Slider';
import Specialities from './home-sections/Specialities';
import Blogs from './home-sections/Blogs';

export default class Home extends Component {
  static displayName = Home.name;

  static logo = require('../resources/images/logo.svg');

  render(): JSX.Element {
    return (
      <div className="App App-home header-distance">
        <Slider />
        <Specialities />
        <Blogs />
        <Logo className="App-logo" />
        <img src={logo} className="App-logo" alt="logo" />
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </div>
    );
  }
}
