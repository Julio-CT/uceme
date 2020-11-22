import * as React from 'react'
import { Container } from 'reactstrap';
import './Layout.css';
import { NavMenu } from './NavMenu';

export class Layout extends React.Component {
  static displayName = Layout.name;

  componentDidMount() {
    this.headerShrinker();
  }

  render() {
    return (
      <div>
        <NavMenu />
        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }

  private headerShrinker(): void {
    window.addEventListener('scroll', function () {
      var distanceY = window.pageYOffset || document.documentElement.scrollTop, shrinkOn = 50;
      if (distanceY > shrinkOn) {
        Array.from(document.getElementsByClassName('big-logo')).forEach(function (item: any) { item && item.classList.add('small-logo'); });
        Array.from(document.getElementsByClassName('site-title-div')).forEach(function (item: any) { item && item.classList.add('site-title-div-small'); });
      }
      else {
        Array.from(document.getElementsByClassName('small-logo')).forEach(function (item: any) { item && item.classList.remove('small-logo'); });
        Array.from(document.getElementsByClassName('site-title-div-small')).forEach(function (item: any) { item && item.classList.remove('site-title-div-small'); });
      }
    });
  }
}
