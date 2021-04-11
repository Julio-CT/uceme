import * as React from 'react';
import { Button, ButtonGroup } from 'reactstrap';
import './AppointmentModal.scss';

type AppointmentHoursProps = {
    hours: string[];
    selectedHour?: string;
    onSelectedHour: (v: any) => void;
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

export const MyButtonGroup = React.forwardRef((props: any, ref: React.Ref<any>) => {
    return (
        <>
            <ButtonGroup {...props} ref={ref} />
        </>
    )
});

class AppointmentHours extends React.Component<AppointmentHoursProps, AppointmentHoursState> {
    refScrollable: any;
    leftPaddle: any;
    rightPaddle: any;
    constructor(props: AppointmentHoursProps) {
        super(props);
        this.state = {
            hours: this.props.hours,
            selectedHour: this.props.selectedHour,
            leftPaddleEnabled: false,
            rightPaddleEnabled: true,
        };
        this.refScrollable = React.createRef();
        this.leftPaddle = React.createRef();
        this.rightPaddle = React.createRef();
    }

    // get how much have we scrolled to the left
    getMenuPosition = () => {
        return this.refScrollable.current.scrollLeft;
    };

    // duration of scroll animation
    scrollDuration = 300;

    setSideScroll = () => {
        if (this.state.hours.length > 0) {
            // get items dimensions
            const itemsLength = document.getElementsByClassName('scrollable-item').length;
            const itemSize = document.getElementsByClassName('scrollable-item')[0].clientWidth;
            // get some relevant size for the paddle triggering point
            const paddleMargin = 20;

            // get wrapper width
            const getMenuWrapperSize = function () {
                return document.getElementsByClassName('scrollable-wrapper')[0].clientWidth;
            }

            let menuWrapperSize = getMenuWrapperSize();

            // the wrapper is responsive
            window.addEventListener('resize', (event) => {
                menuWrapperSize = getMenuWrapperSize();
            });

            // get total width of all menu items
            const getMenuSize = function () {
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
                var menuPosition = this.getMenuPosition();

                var menuEndOffset = menuInvisibleSize - paddleMargin;

                // show & hide the paddles 
                // depending on scroll position
                if (menuPosition <= paddleMargin) {
                    /* this.leftPaddle.current.classList.add('hidden');
                    this.rightPaddle.current.classList.remove('hidden'); */
                    this.setState({ leftPaddleEnabled: false, rightPaddleEnabled: true, });
                } else if (menuPosition < menuEndOffset) {
                    // show both paddles in the middle
                    /* this.leftPaddle.current.classList.remove('hidden');
                    this.rightPaddle.current.classList.remove('hidden'); */
                    this.setState({ leftPaddleEnabled: true, rightPaddleEnabled: true, });
                } else if (menuPosition >= menuEndOffset) {
                    /* this.leftPaddle.current.classList.remove('hidden');
                    this.rightPaddle.current.classList.add('hidden'); */
                    this.setState({ leftPaddleEnabled: true, rightPaddleEnabled: false, });
                }
            });
        }
    }

    handleRightPaddleClick = (amount: number, e: any) => {
        e.preventDefault();
        if (this.refScrollable.current != null) {
            this.refScrollable.current.scrollLeft += amount;
            this.refScrollable.current.animate({ scrollLeft: this.getMenuPosition() + amount }, this.scrollDuration);
        }
    };

    selectHour = (hour: string) => {
        this.setState({
            selectedHour: hour
        }, () => this.props.onSelectedHour(hour));
    }

    componentDidUpdate(newProps: AppointmentHoursProps) {
        if (this.props.hours.length !== this.state.hours.length) {
            this.setState({
                hours: newProps.hours,
                selectedHour: newProps.selectedHour,
            }, () => {
                this.setSideScroll();
            })
        }
    }

    render() {
        return (
            <div className="scrollable-wrapper">
                <div className="scrollable" ref={this.refScrollable}>
                    <MyButtonGroup id="hourForm" >
                        {this.state.hours.map((hour: string) => {
                            return (
                                <Button key={hour}
                                    onClick={() => this.selectHour(hour)}
                                    active={hour === this.state.selectedHour}
                                    className="scrollable-item">
                                    {hour}
                                </Button>)
                            })
                        }
                    </MyButtonGroup>
                </div>
                <div className="paddles">
                    <button className="left-paddle paddle" disabled={!this.state.leftPaddleEnabled} onClick={(e) => this.handleRightPaddleClick(-20, e)} ref={this.leftPaddle}>
                        {'<'}
                    </button>
                    <button className="right-paddle paddle" disabled={!this.state.rightPaddleEnabled} onClick={(e) => this.handleRightPaddleClick(20, e)} ref={this.rightPaddle}>
                        {'>'}
                    </button>
                </div>
            </div>)
    }
}

export default AppointmentHours;
