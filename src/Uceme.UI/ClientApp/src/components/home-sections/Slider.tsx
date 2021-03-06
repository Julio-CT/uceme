import * as React from 'react';
import { useState } from 'react';
import {
  Carousel,
  CarouselItem,
  CarouselControl,
  CarouselIndicators,
  CarouselCaption,
} from 'reactstrap';
import './Slider.scss';
import slide1 from '../../resources/images/uceme.jpg';
import slide2 from '../../resources/images/innovaciones-tecnicas.jpg';
import slide3 from '../../resources/images/conocenos-uceme.jpg';
import slide4 from '../../resources/images/cita-medica-uceme.jpg';

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

const Slider: () => JSX.Element = () => {
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

  const slides: JSX.Element[] = items.map((item) => {
    return (
      <CarouselItem
        onExiting={() => setAnimating(true)}
        onExited={() => setAnimating(false)}
        key={item.src}
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
