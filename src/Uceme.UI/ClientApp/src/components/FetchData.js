"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const AuthorizeService_1 = require("./api-authorization/AuthorizeService");
class FetchData extends React.Component {
    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true };
    }
    componentDidMount() {
        this.populateWeatherData();
    }
    static renderForecastsTable(forecasts) {
        return (React.createElement("table", { className: 'table table-striped', "aria-labelledby": "tabelLabel" },
            React.createElement("thead", null,
                React.createElement("tr", null,
                    React.createElement("th", null, "Date"),
                    React.createElement("th", null, "Temp. (C)"),
                    React.createElement("th", null, "Temp. (F)"),
                    React.createElement("th", null, "Summary"))),
            React.createElement("tbody", null, forecasts.map(forecast => React.createElement("tr", { key: forecast.date },
                React.createElement("td", null, forecast.date),
                React.createElement("td", null, forecast.temperatureC),
                React.createElement("td", null, forecast.temperatureF),
                React.createElement("td", null, forecast.summary))))));
    }
    render() {
        let contents = this.state.loading
            ? React.createElement("p", null,
                React.createElement("em", null, "Loading..."))
            : FetchData.renderForecastsTable(this.state.forecasts);
        return (React.createElement("div", null,
            React.createElement("h1", { id: "tabelLabel" }, "Weather forecast"),
            React.createElement("p", null, "This component demonstrates fetching data from the server."),
            contents));
    }
    populateWeatherData() {
        return __awaiter(this, void 0, void 0, function* () {
            const token = yield AuthorizeService_1.default.getAccessToken();
            const response2 = yield fetch('http://localhost:5000/home/getmedicominvista', {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
            });
            const data2 = yield response2.json();
            const response = yield fetch('weatherforecast', {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
            });
            const data = yield response.json();
            this.setState({ forecasts: data, loading: false });
        });
    }
}
exports.FetchData = FetchData;
FetchData.displayName = FetchData.name;
//# sourceMappingURL=FetchData.js.map