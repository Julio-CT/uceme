import * as React from 'react'

type CounterState = {
    currentCount: number
}

export class Counter extends React.Component<{}, CounterState> {
    static displayName = Counter.name;

    constructor(props: any) {
        super(props);
        this.state = { currentCount: 0 };
        this.incrementCounter = this.incrementCounter.bind(this);
    }

    incrementCounter() {
        this.setState({
            currentCount: this.state.currentCount + 1
        });
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    render() {
        return (
            <div>
                <h1>Counter</h1>

                <p>This is a simple example of a React component.</p>

                <p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>

                <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
            </div>
        );
    }

    async populateWeatherData() {
        const response3 = await fetch('home/getmedicominvista');
        const data3 = await response3.json();
        console.log(data3);
    }
}
