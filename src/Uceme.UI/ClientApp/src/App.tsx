import * as React from 'react'
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import FetchData from './components/FetchData';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import ApplicationPaths from './components/api-authorization/ApiAuthorizationConstants';
import './App.scss';
import './custom.scss';

function App(): JSX.Element {
    return (
        <Layout>
            <Route exact path='/' component={Home} />
            <Route path='/especialidades' component={Home} />
            <Route path='/innovaciones' component={Home} />
            <Route path='/blog' component={Home} />
            <Route path='/contacto' component={Home} />
            <AuthorizeRoute path='/fetch-data' component={FetchData} />
            <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
        </Layout>
    );
}

export default App;
