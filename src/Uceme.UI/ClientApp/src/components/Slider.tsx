import * as React from 'react';
import { useState } from 'react';
import {
    Carousel,
    CarouselItem,
    CarouselControl,
    CarouselIndicators,
    CarouselCaption
} from 'reactstrap';
import './Slider.css';
import logo1 from '../resources/images/logo.svg';
import logo2 from '../resources/images/logo2.svg';
import logo3 from '../resources/images/logo3.svg';

const items = [
    {
        src: logo1,
        altText: 'Slide 1',
        caption: 'Slide 1'
    },
    {
        src: logo2,
        altText: 'Slide 2',
        caption: 'Slide 2'
    },
    {
        src: logo3,
        altText: 'Slide 3',
        caption: 'Slide 3'
    }
];

const populateWeatherData = async () => {
    const response3 = await fetch('home/getmedicominvista');
    const data3 = await response3.json();
    console.log(data3);
}

const Slider = (props: any) => {
    const [activeIndex, setActiveIndex] = useState(0);
    const [animating, setAnimating] = useState(false);

    const next = () => {
        if (animating) return;
        const nextIndex = activeIndex === items.length - 1 ? 0 : activeIndex + 1;
        setActiveIndex(nextIndex);
    }

    const previous = () => {
        if (animating) return;
        const nextIndex = activeIndex === 0 ? items.length - 1 : activeIndex - 1;
        setActiveIndex(nextIndex);
    }

    const goToIndex = (newIndex: React.SetStateAction<number>) => {
        if (animating) return;
        setActiveIndex(newIndex);
    }

    React.useEffect(() => {
        populateWeatherData();
    });

    const slides = items.map((item) => {
        return (
            <CarouselItem
                onExiting={() => setAnimating(true)}
                onExited={() => setAnimating(false)}
                key={item.src}
            >
                <img src={item.src} alt={item.altText} />
                <CarouselCaption captionText={item.caption} captionHeader={item.caption} />
            </CarouselItem>
        );
    });

    return (
        <Carousel
            activeIndex={activeIndex}
            next={next}
            previous={previous}
        >
            <CarouselIndicators items={items} activeIndex={activeIndex} onClickHandler={goToIndex} />
            {slides}
            <CarouselControl direction="prev" directionText="Previous" onClickHandler={previous} />
            <CarouselControl direction="next" directionText="Next" onClickHandler={next} />
        </Carousel>
    );
}

export default Slider;
