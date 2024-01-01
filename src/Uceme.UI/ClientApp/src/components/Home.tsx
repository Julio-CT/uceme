import * as React from 'react';
import './Home.scss';
import { ReactElement } from 'react';
import Slider from './home-sections/Slider';
import Specialities from './home-sections/Specialities';
import Blogs from './home-sections/Blogs';
import ContactUs from './home-sections/ContactUs';

function Home(): ReactElement {
  return (
    <div className="app app-home header-distance-l">
      <Slider />
      <Specialities />
      <Blogs />
      <ContactUs />
    </div>
  );
}

export default Home;
