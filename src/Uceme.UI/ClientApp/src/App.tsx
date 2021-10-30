import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import BlogHome from './components/BlogHome';
import Conditions from './components/Conditions';
import BlogItem from './components/BlogPost';
import Specialities from './components/Specialities';
import ContactUs from './components/ContactUs';
import AppointmentManager from './components/admin/AppointmentManager';
import PostManager from './components/admin/PostManager';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import ApplicationPaths from './components/api-authorization/ApiAuthorizationConstants';
import './custom.scss';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import './App.scss';

function App(): JSX.Element {
  return (
    <Layout>
      <Route exact path="/" component={Home} />
      <Route path="/condiciones" component={Conditions} />
      <Route path="/especialidades" component={Specialities} />
      <Route path="/innovaciones" component={Home} />
      <Route path="/blog/:page?" component={BlogHome} />
      <Route path="/post/:slug" component={BlogItem} />
      <Route path="/contacto" component={ContactUs} />
      <AuthorizeRoute
        path="/appointmentmanager"
        component={AppointmentManager}
      />
      <AuthorizeRoute path="/postmanager" component={PostManager} />
      <Route
        path={ApplicationPaths.ApiAuthorizationPrefix}
        component={ApiAuthorizationRoutes}
      />
      <Route path="/adminlogin" component={ApiAuthorizationRoutes} />
    </Layout>
  );
}

export default App;
