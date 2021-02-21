import * as React from 'react';
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';
import './AppointmentModal.scss';

const AppointmentModal = (): JSX.Element => {
  const [modal, setModal] = React.useState(false);

  const toggle = () => setModal(!modal);
  return (
    <Modal isOpen={modal} toggle={toggle}>
      <ModalHeader toggle={toggle}>
        <div className="e1p32jev1 css-1ij5pq9 e6vs4hd0">
          <picture>
            <source
              srcSet="Les%20Pe%CC%81pites%20in%20Paris%20-%20Restaurant%20Reviews,%20Menu%20and%20Prices%20-%20TheFork_files/adafd213-7c06-4add-bb62-2e12be1e0eb5_002.webp 800w, Les%20Pe%CC%81pites%20in%20Paris%20-%20Restaurant%20Reviews,%20Menu%20and%20Prices%20-%20TheFork_files/adafd213-7c06-4add-bb62-2e12be1e0eb5_006.webp 1600w"
              media="(min-width:400px)"
              sizes="800px"
            ></source>
            <source
              srcSet="Les%20Pe%CC%81pites%20in%20Paris%20-%20Restaurant%20Reviews,%20Menu%20and%20Prices%20-%20TheFork_files/adafd213-7c06-4add-bb62-2e12be1e0eb5_003.webp 400w, Les%20Pe%CC%81pites%20in%20Paris%20-%20Restaurant%20Reviews,%20Menu%20and%20Prices%20-%20TheFork_files/adafd213-7c06-4add-bb62-2e12be1e0eb5_005.webp 800w"
              sizes="400px"
            ></source>
            <img
              alt=""
              src="Les%20Pe%CC%81pites%20in%20Paris%20-%20Restaurant%20Reviews,%20Menu%20and%20Prices%20-%20TheFork_files/adafd213-7c06-4add-bb62-2e12be1e0eb5_003.webp"
              className="e316tjz1 css-1pyjdiy e316tjz0"
              width="400"
              height="200"
            ></img>
          </picture>
          <div className="css-1psewps e6vs4hd0"></div>
          <div color="special.white" className="css-1ao3py0 e6vs4hd0">
            <div className="css-19h4hyn e6vs4hd0">
              <h2 color="special.white" className="css-p6idae ejesmtr0">
                <span className="css-13rhg2p et6f7t80">
                  <span>Reservation</span>
                  <span
                    aria-hidden="true"
                    className="css-ig1rp0 e1vdlbgy0"
                  ></span>
                </span>
                Les Pépites
              </h2>
            </div>
          </div>
        </div>
      </ModalHeader>
      <ModalBody>
        Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod
        tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim
        veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea
        commodo consequat. Duis aute irure dolor in reprehenderit in voluptate
        velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint
        occaecat cupidatat non proident, sunt in culpa qui officia deserunt
        mollit anim id est laborum.
      </ModalBody>
      <ModalFooter>
        <Button color="primary" onClick={toggle}>
          Do Something
        </Button>{' '}
        <Button color="secondary" onClick={toggle}>
          Cancel
        </Button>
      </ModalFooter>
    </Modal>
  );
};

export default AppointmentModal;
