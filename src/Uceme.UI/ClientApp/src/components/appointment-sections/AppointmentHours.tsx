import * as React from 'react';
import { Button, ButtonGroup } from 'reactstrap';
import './AppointmentModal.scss';

type AppointmentHoursProps = {
  hours: string[];
  selectedHour?: string;
  onSelectedHour: (v: string) => void;
  children?: React.ReactElement[];
  params?: any;
  history?: any;
  location?: any;
  match?: any;
};

type AppointmentHoursState = {
  hours: string[];
  selectedHour?: string;
  leftPaddleEnabled: boolean;
  rightPaddleEnabled: boolean;
};

type MyButtonGroupProps = {
  id: string;
  children: JSX.Element[] | JSX.Element;
};

export const MyButtonGroup = React.forwardRef(
  (props: MyButtonGroupProps, ref: React.Ref<any>) => {
    return (
      <>
        <ButtonGroup id={props.id} ref={ref}>
          {props.children}
        </ButtonGroup>
      </>
    );
  }
);

class AppointmentHours extends React.Component<
  AppointmentHoursProps,
  AppointmentHoursState
> {
  refScrollable: React.RefObject<any>;

  leftPaddle: React.RefObject<any>;

  rightPaddle: React.RefObject<any>;

  // duration of scroll animation
  scrollDuration = 300;

  onSelectedHour: (v: string) => void;

  constructor(props: AppointmentHoursProps) {
    super(props);
    const { hours, selectedHour, onSelectedHour } = this.props;
    this.state = {
      hours,
      selectedHour,
      leftPaddleEnabled: false,
      rightPaddleEnabled: true,
    };
    this.onSelectedHour = onSelectedHour;
    this.refScrollable = React.createRef();
    this.leftPaddle = React.createRef();
    this.rightPaddle = React.createRef();
  }

  // get how much have we scrolled to the left
  getMenuPosition: () => number = () => {
    return this.refScrollable.current.scrollLeft;
  };

  setSideScroll: () => void = () => {
    const { hours } = this.state;
    if (hours.length > 0) {
      // get items dimensions
      const itemsLength = document.getElementsByClassName('scrollable-item')
        .length;
      const itemSize = document.getElementsByClassName('scrollable-item')[0]
        .clientWidth;
      // get some relevant size for the paddle triggering point
      const paddleMargin = 20;

      // get wrapper width
      const getMenuWrapperSize: () => number = () => {
        return document.getElementsByClassName('scrollable-wrapper')[0]
          .clientWidth;
      };

      let menuWrapperSize = getMenuWrapperSize();

      // the wrapper is responsive
      window.addEventListener('resize', () => {
        menuWrapperSize = getMenuWrapperSize();
      });

      // get total width of all menu items
      const getMenuSize: () => number = () => {
        return itemsLength * itemSize;
      };

      const menuSize = getMenuSize();
      // get how much of menu is invisible
      let menuInvisibleSize = menuSize - menuWrapperSize;

      // finally, what happens when we are actually scrolling the menu
      this.refScrollable.current.addEventListener('scroll', () => {
        // get how much of menu is invisible
        menuInvisibleSize = menuSize - menuWrapperSize;
        // get how much have we scrolled so far
        const menuPosition = this.getMenuPosition();

        const menuEndOffset = menuInvisibleSize - paddleMargin;

        // show & hide the paddles
        // depending on scroll position
        if (menuPosition <= paddleMargin) {
          /* this.leftPaddle.current.classList.add('hidden');
                    this.rightPaddle.current.classList.remove('hidden'); */
          this.setState({ leftPaddleEnabled: false, rightPaddleEnabled: true });
        } else if (menuPosition < menuEndOffset) {
          // show both paddles in the middle
          /* this.leftPaddle.current.classList.remove('hidden');
                    this.rightPaddle.current.classList.remove('hidden'); */
          this.setState({ leftPaddleEnabled: true, rightPaddleEnabled: true });
        } else if (menuPosition >= menuEndOffset) {
          /* this.leftPaddle.current.classList.remove('hidden');
                    this.rightPaddle.current.classList.add('hidden'); */
          this.setState({ leftPaddleEnabled: true, rightPaddleEnabled: false });
        }
      });
    }
  };

  handleRightPaddleClick: (amount: number, e: any) => void = (
    amount: number,
    e: any
  ) => {
    e.preventDefault();
    if (this.refScrollable.current != null) {
      this.refScrollable.current.scrollLeft += amount;
      this.refScrollable.current.animate(
        { scrollLeft: this.getMenuPosition() + amount },
        this.scrollDuration
      );
    }
  };

  selectHour: (hour: string) => void = (hour: string) => {
    this.setState(
      {
        selectedHour: hour,
      },
      () => this.onSelectedHour(hour)
    );
  };

  render(): JSX.Element {
    const {
      hours,
      selectedHour,
      leftPaddleEnabled,
      rightPaddleEnabled,
    } = this.state;
    return (
      <div className="scrollable-wrapper">
        <div className="scrollable" ref={this.refScrollable}>
          <MyButtonGroup id="hourForm">
            {hours.map((hour: string) => {
              return (
                <Button
                  key={hour}
                  onClick={() => this.selectHour(hour)}
                  active={hour === selectedHour}
                  className="scrollable-item"
                >
                  {hour}
                </Button>
              );
            })}
          </MyButtonGroup>
        </div>
        <div className="paddles">
          <button
            className="left-paddle paddle"
            disabled={!leftPaddleEnabled}
            onClick={(e) => this.handleRightPaddleClick(-20, e)}
            ref={this.leftPaddle}
            type="button"
          >
            {'<'}
          </button>
          <button
            className="right-paddle paddle"
            disabled={!rightPaddleEnabled}
            onClick={(e) => this.handleRightPaddleClick(20, e)}
            ref={this.rightPaddle}
            type="button"
          >
            {'>'}
          </button>
        </div>
      </div>
    );
  }
}

export default AppointmentHours;
