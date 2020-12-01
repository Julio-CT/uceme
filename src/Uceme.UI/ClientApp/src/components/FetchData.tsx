import * as React from 'react';
import authService from './api-authorization/AuthorizeService';

type FetchDataState = {
  forecasts: any[];
  loading: boolean;
};

export default class FetchData extends React.Component<
  Record<string, unknown>,
  FetchDataState
> {
  static displayName = FetchData.name;

  constructor(props: Record<string, unknown>) {
    super(props);
    this.state = { forecasts: [], loading: true };
  }

  componentDidMount(): void {
    this.populateWeatherData();
  }

  static renderForecastsTable(forecasts: Array<any>): JSX.Element {
    return (
      <table className="table table-striped" aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map((forecast) => (
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  }

  async populateWeatherData(): Promise<void> {
    const token = await authService.getAccessToken();
    const response2 = await fetch('home/getmedicominvista', {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    });
    const data2 = await response2.json();
    console.log(data2);

    const response = await fetch('weatherforecast', {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    });
    const data = await response.json();
    this.setState({ forecasts: data, loading: false });
  }

  render(): JSX.Element {
    const contents = this.state.loading ? (
      <p>
        <em>Loading...</em>
      </p>
    ) : (
      FetchData.renderForecastsTable(this.state.forecasts)
    );

    return (
      <div>
        <h1 id="tabelLabel">Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }
}
