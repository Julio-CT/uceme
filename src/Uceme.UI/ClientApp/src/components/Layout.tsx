import * as React from 'react';
import './Layout.scss';
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
          function makeLogoSmall(item: Element): void {
            item && item.classList.add('small-logo');
          }
        );
        Array.from(document.getElementsByClassName('site-title-div')).forEach(
          function makeTitleSmall(item: Element): void {
            item && item.classList.add('site-title-div-small');
          }
        );
      } else {
        Array.from(document.getElementsByClassName('small-logo')).forEach(
          function makeLogoBig(item: Element): void {
            item && item.classList.remove('small-logo');
          }
        );
        Array.from(
          document.getElementsByClassName('site-title-div-small')
        ).forEach(function makeTitleBig(item: Element): void {
          item && item.classList.remove('site-title-div-small');
        });
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

  render(): JSX.Element {
    const { children } = this.state;
    return (
      <>
        <Header />
        <>{children}</>
        <Footer />
      </>
    );
  }
}
