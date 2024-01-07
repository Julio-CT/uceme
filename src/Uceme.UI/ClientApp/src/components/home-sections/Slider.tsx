/* eslint-disable react/function-component-definition */
import * as React from 'react';
import { ReactElement, useState } from 'react';
import {
  Carousel,
  CarouselItem,
  CarouselControl,
  CarouselIndicators,
  CarouselCaption,
} from 'reactstrap';
import './Slider.scss';
import slide1 from '../../resources/images/uceme.webp';
import slide2 from '../../resources/images/innovaciones-tecnicas.webp';
import slide3 from '../../resources/images/conocenos-uceme.webp';
import slide4 from '../../resources/images/cita-medica-uceme.webp';

const items = [
  {
    src: slide1,
    altText: 'Unidad de Cirugía Endocrinometabólica Especializada',
    caption: 'UCEME',
  },
  {
    src: slide2,
    altText: 'Pioneros en España en Tiroidectomía por Abordaje Extracervical',
    caption: 'INNOVACIONES TÉCNICAS',
  },
  {
    src: slide3,
    altText:
      'Ofrecemos un trato personalizado en enfermedades de tiroides, paratiroides, glándula suprarrenal y obesidad mórbida.',
    caption: 'CONÓCENOS',
  },
  {
    src: slide4,
    altText: 'Sistema de cita previa online',
    caption: 'CITA PREVIA',
  },
];

const Slider: () => ReactElement = () => {
  const [activeIndex, setActiveIndex] = useState(0);
  const [animating, setAnimating] = useState(false);

  const next = () => {
    if (animating) return;
    const nextIndex = activeIndex === items.length - 1 ? 0 : activeIndex + 1;
    setActiveIndex(nextIndex);
  };

  const previous = () => {
    if (animating) return;
    const nextIndex = activeIndex === 0 ? items.length - 1 : activeIndex - 1;
    setActiveIndex(nextIndex);
  };

  const goToIndex = (newIndex: React.SetStateAction<number>) => {
    if (animating) return;
    setActiveIndex(newIndex);
  };

  const slides: ReactElement[] = items.map((item) => {
    return (
      <CarouselItem
        onExiting={() => setAnimating(true)}
        onExited={() => setAnimating(false)}
        key={item.caption}
      >
        <img src={item.src} alt={item.altText} />
        <CarouselCaption
          captionText={item.altText}
          captionHeader={item.caption}
        />
      </CarouselItem>
    );
  });

  return (
    <section id="section-slide" className="clearfix">
      <Carousel activeIndex={activeIndex} next={next} previous={previous}>
        <CarouselIndicators
          items={items}
          activeIndex={activeIndex}
          onClickHandler={goToIndex}
        />
        {slides}
        <CarouselControl
          direction="prev"
          directionText="Previous"
          onClickHandler={previous}
        />
        <CarouselControl
          direction="next"
          directionText="Next"
          onClickHandler={next}
        />
      </Carousel>
    </section>
  );
};

export default Slider;
