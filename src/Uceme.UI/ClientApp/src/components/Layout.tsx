import * as React from 'react';
import './Layout.scss';
import { ReactElement } from 'react';
import Header from './Header';
import Footer from './Footer';

type LayoutState = {
  children: React.ReactElement[];
};

type LayoutProps = {
  children: React.ReactElement[];
};

export default class Layout extends React.Component<LayoutProps, LayoutState> {
  static headerShrinker(): void {
    window.addEventListener('scroll', function scrollListener(): void {
      const distanceY =
        window.pageYOffset || document.documentElement.scrollTop;
      const shrinkOn = 50;
      if (distanceY > shrinkOn) {
        Array.from(document.getElementsByClassName('big-logo')).forEach(
          (item: Element): void => item && item.classList.add('small-logo')
        );
        Array.from(document.getElementsByClassName('site-title-div')).forEach(
          (item: Element): void =>
            item && item.classList.add('site-title-div-small')
        );
      } else {
        Array.from(document.getElementsByClassName('small-logo')).forEach(
          (item: Element): void => item && item.classList.remove('small-logo')
        );
        Array.from(
          document.getElementsByClassName('site-title-div-small')
        ).forEach(
          (item: Element): void =>
            item && item.classList.remove('site-title-div-small')
        );
      }
    });
  }

  constructor(props: Readonly<LayoutProps>) {
    super(props);

    this.state = {
      children: props.children,
    };
  }

  componentDidMount(): void {
    Layout.headerShrinker();
  }

  render(): ReactElement {
    const { children } = this.state;
    return (
      <>
        <Header />
        {children}
        <Footer />
      </>
    );
  }
}
