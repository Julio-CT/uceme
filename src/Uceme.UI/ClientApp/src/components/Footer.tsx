import * as React from 'react';
import { Container } from 'reactstrap';
import './Footer.scss';

const Footer = (): JSX.Element => {
  const year = new Date().getFullYear();
  return (
    <footer className="padded">
      <Container>
        <div className="full-width">
          <div id="links" className="float-left socialmediafooter">
            <div className="socialmedia">
              <a
                href="http://goo.gl/AY2H7"
                className="facebook"
                target="_blank"
                rel="noreferrer"
              >
                facebook
              </a>
            </div>
            <div className="socialmedia">
              <a
                href="http://goo.gl/rX37S"
                className="twitter"
                target="_blank"
                rel="noreferrer"
              >
                twitter
              </a>
            </div>
            <div className="socialmedia">
              <a
                href="http://goo.gl/MDk9n"
                className="linkedin"
                target="_blank"
                rel="noreferrer"
              >
                linkedin
              </a>
            </div>
            <div className="socialmedia">
              <a
                href="https://www.instagram.com/endocrinologiamadrid/"
                className="instagram"
                target="_blank"
                rel="noreferrer"
              >
                instagram
              </a>
            </div>
          </div>
          <p className="creditos float-left">
            &copy; {year} - Unidad de Cirugía Endocrinometabólica Especializada.
            <span>Madrid</span>
            <br />
          </p>
          <a
            href="http://validator.w3.org/check?uri=http%3A%2F%2Fwww.uceme.es%2F"
            target="_blank"
            className="float-right"
            rel="noreferrer"
          >
            <img
              src="http://www.w3.org/html/logo/badge/html5-badge-h-css3-semantics.png"
              width="65px"
              alt="HTML5 Powered with CSS3 / Styling, and Semantics"
              title="HTML5 Powered with CSS3 / Styling, and Semantics"
            />
          </a>
        </div>
      </Container>
    </footer>
  );
};

export default Footer;
