import * as React from 'react'
import { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
        <div className="App">
            <header className="App-header">
            <img src="./logo.svg" className="App-logo" alt="logo" />
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
            </header>
        </div>
    );
  }
}
