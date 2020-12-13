import * as React from 'react';
import './Layout.scss';
import NavMenu from './NavMenu';

type LayoutState = {
  children: any;
};

type LayoutProps = {
  children: any;
};

export default class Layout extends React.Component<LayoutProps, LayoutState> {
  static displayName = Layout.name;

  constructor(props: Readonly<LayoutProps>) {
    super(props);

    this.state = {
      children: props.children,
    };
  }

  componentDidMount(): void {
    this.headerShrinker();
  }

  headerShrinker(): void {
    window.addEventListener('scroll', function scrollListener(): void {
      const distanceY =
        window.pageYOffset || document.documentElement.scrollTop;
      const shrinkOn = 50;
      if (distanceY > shrinkOn) {
        Array.from(document.getElementsByClassName('big-logo')).forEach(
          function makeLogoSmall(item: any): void {
            item && item.classList.add('small-logo');
          }
        );
        Array.from(document.getElementsByClassName('site-title-div')).forEach(
          function makeTitleSmall(item: any): void {
            item && item.classList.add('site-title-div-small');
          }
        );
      } else {
        Array.from(document.getElementsByClassName('small-logo')).forEach(
          function makeLogoBig(item: any): void {
            item && item.classList.remove('small-logo');
          }
        );
        Array.from(
          document.getElementsByClassName('site-title-div-small')
        ).forEach(function makeTitleBig(item: any): void {
          item && item.classList.remove('site-title-div-small');
        });
      }
    });
  }

  render(): JSX.Element {
    const { children } = this.state;
    return (
      <React.Fragment>
        <NavMenu />
        <React.Fragment>{children}</React.Fragment>
      </React.Fragment>
    );
  }
}
