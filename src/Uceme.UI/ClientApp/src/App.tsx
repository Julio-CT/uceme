import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import BlogHome from './components/BlogHome';
import ConditionsComponent from './components/ConditionsComponent';
import BlogPostComponent from './components/BlogPostComponent';
import SpecialitiesComponent from './components/SpecialitiesComponent';
import ContactUsComponent from './components/ContactUsComponent';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import ApplicationPaths from './components/api-authorization/ApiAuthorizationConstants';
import './App.scss';
import './custom.scss';

function App(): JSX.Element {
  return (
    <Layout>
      <Route exact path="/" component={Home} />
      <Route path="/condiciones" component={ConditionsComponent} />
      <Route path="/especialidades" component={SpecialitiesComponent} />
      <Route path="/innovaciones" component={Home} />
      <Route path="/blog/:page?" component={BlogHome} />
      <Route path="/post/:slug" component={BlogPostComponent} />
      <Route path="/contacto" component={ContactUsComponent} />
      <Route
        path={ApplicationPaths.ApiAuthorizationPrefix}
        component={ApiAuthorizationRoutes}
      />
      <Route
        path="/adminlogin"
        component={ApiAuthorizationRoutes}
      />
    </Layout>
  );
}

export default App;
