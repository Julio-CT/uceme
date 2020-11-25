import * as React from 'react'
import { Component } from 'react';
import logo from '../resources/images/logo.svg';
import { ReactComponent as Logo } from '../resources/images/logo.svg';
import './Home.css';
import Slider from './Slider';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div className="App App-home header-distance">
                <Slider></Slider>
                <Logo className="App-logo" />
                <img src={require('../resources/images/logo.svg')} className="App-logo" alt="logo" />
                <img src={logo} className="App-logo" alt="logo" />
                <p>
                    Edit <code>src/App.tsx</code> and save to reload.
                </p>
                <a
                    className="App-link"
                    href="https://reactjs.org"
                    target="_blank"
                    rel="noopener noreferrer">
                    Learn React
            </a>
            </div>
        );
    }
}
