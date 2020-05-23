"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
class Counter extends React.Component {
    constructor(props) {
        super(props);
        this.state = { currentCount: 0 };
        this.incrementCounter = this.incrementCounter.bind(this);
    }
    incrementCounter() {
        this.setState({
            currentCount: this.state.currentCount + 1
        });
    }
    render() {
        return (React.createElement("div", null,
            React.createElement("h1", null, "Counter"),
            React.createElement("p", null, "This is a simple example of a React component."),
            React.createElement("p", { "aria-live": "polite" },
                "Current count: ",
                React.createElement("strong", null, this.state.currentCount)),
            React.createElement("button", { className: "btn btn-primary", onClick: this.incrementCounter }, "Increment")));
    }
}
exports.Counter = Counter;
Counter.displayName = Counter.name;
//# sourceMappingURL=Counter.js.map