import * as React from 'react';
import { render, screen } from '@testing-library/react';
import Specialities from './Specialities';
import { unmountComponentAtNode } from 'react-dom';

let container: any;
beforeEach(() => {
  // setup a DOM element as a render target
  container = document.createElement('div');
  document.body.appendChild(container);
});

afterEach(() => {
  // cleanup on exiting
  unmountComponentAtNode(container);
  container.remove();
  container = null;
});

describe('(Component)) Specialities', () => {
  it('renders without exploding', () => {
    render(<Specialities />, container);
    const image = screen.getByAltText('Cirugia');
    expect(image).toBeTruthy();
  });

  it('renders all images', () => {
    render(<Specialities />, container);
    const image1 = screen.getByAltText('Cirugia');
    expect(image1).toBeTruthy();
    const image2 = screen.getByAltText('Paratiroidea');
    expect(image2).toBeTruthy();
    const image3 = screen.getByAltText('Obesidad');
    expect(image3).toBeTruthy();
    const image4 = screen.getByAltText('Suprarrenales');
    expect(image4).toBeTruthy();
    const image5 = screen.getByAltText('Tiroidectomia');
    expect(image5).toBeTruthy();
  });
});
