import * as React from 'react';
import { render, screen } from '@testing-library/react';
import Slider from './Slider';
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

describe('(Component)) Slider', () => {
  it('renders without exploding', () => {
    render(<Slider />, container);
    const image = screen.getByAltText(
      'Unidad de Cirugía Endocrinometabólica Especializada'
    );
    expect(image).toBeTruthy();
  });

  it('renders all images', () => {
    render(<Slider />, container);
    const image1 = screen.getByAltText(
      'Unidad de Cirugía Endocrinometabólica Especializada'
    );
    expect(image1).toBeTruthy();
    const image2 = screen.getByAltText(
      'Pioneros en España en Tiroidectomía por Abordaje Extracervical'
    );
    expect(image2).toBeTruthy();
    const image3 = screen.getByAltText(
      'Ofrecemos un trato personalizado en enfermedades de tiroides, paratiroides, glándula suprarrenal y obesidad mórbida.'
    );
    expect(image3).toBeTruthy();
    const image4 = screen.getByAltText('Sistema de cita previa online');
    expect(image4).toBeTruthy();
  });
});
