import * as React from 'react';
import { Component } from 'react';

import './Home.scss';
import Slider from './home-sections/Slider';
import Specialities from './home-sections/Specialities';
import Blogs from './home-sections/Blogs';
import ContactUs from './home-sections/ContactUs';

export default class Home extends Component {
  static displayName = Home.name;

  render(): JSX.Element {
    return (
      <div className="App App-home header-distance">
        <Slider />
        <Specialities />
        <Blogs />
        <ContactUs />
      </div>
    );
  }
}
