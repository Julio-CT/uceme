"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("bootstrap/dist/css/bootstrap.css");
const React = require("react");
const ReactDOM = require("react-dom");
const react_router_dom_1 = require("react-router-dom");
const App_1 = require("./App");
require("./index.css");
const serviceWorker = require("./serviceWorker");
//import registerServiceWorker from './registerServiceWorker';
debugger;
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
//// react stuff
////ReactDOM.render(
////    <React.StrictMode>
////        <App />
////    </React.StrictMode>,
////    document.getElementById('root')
////);
//// vs stuff
ReactDOM.render(React.createElement(react_router_dom_1.BrowserRouter, { basename: baseUrl },
    React.createElement(App_1.default, null)), document.getElementById('root'));
// Uncomment the line above that imports the registerServiceWorker function
// and the line below to register the generated service worker.
// By default create-react-app includes a service worker to improve the
// performance of the application by caching static assets. This service
// worker can interfere with the Identity UI, so it is
// disabled by default when Identity is being used.
// registerServiceWorker();
// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
//# sourceMappingURL=index.js.map