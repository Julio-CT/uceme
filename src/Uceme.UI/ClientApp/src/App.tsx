import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import BlogHome from './components/BlogHome';
import Conditions from './components/Conditions';
import BlogItem from './components/BlogPost';
import Specialities from './components/Specialities';
import Speciality from './components/Speciality';
import ContactUs from './components/ContactUs';
import AppointmentManager from './components/admin/AppointmentManager';
import PostManager from './components/admin/PostManager';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import ApplicationPaths from './components/api-authorization/ApiAuthorizationConstants';
import './custom.scss';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import './App.scss';
import SettingsContext, { Settings } from './SettingsContext';

function App(): JSX.Element {
  const [context, setContext] = React.useState<Settings>({
    telephone: 'hell',
    address: 'yeah',
    contactEmail: 'baby',
    baseHref: process.env.NODE_ENV === 'development' ? '' : 'ucemeapi/',
  });

  const defaults = React.useMemo<Settings>(
    () => ({
      telephone: 'hell',
      address: 'yeah',
      contactEmail: 'baby',
      baseHref: process.env.NODE_ENV === 'development' ? '' : 'ucemeapi/',
    }),
    []
  );

  React.useMemo(() => {
    fetch(`${defaults.baseHref}api/settings/getsettings`, {
      headers: {},
    })
      .then((response: Response) => response.json())
      .then(async (data: Settings) => {
        const newSettings = data;
        if (newSettings) {
          newSettings.baseHref = defaults.baseHref;
        }

        setContext(newSettings);
      });
  }, [defaults.baseHref]);

  return (
    <SettingsContext.Provider value={context}>
      <Layout>
        <Route exact path="/" component={Home} />
        <Route path="/condiciones" component={Conditions} />
        <Route path="/especialidades" component={Specialities} />
        <Route path="/innovaciones" component={Home} />
        <Route path="/blog/:page?" component={BlogHome} />
        <Route path="/post/:slug" component={BlogItem} />
        <Route path="/especialidad/:esp" component={Speciality} />
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
    </SettingsContext.Provider>
  );
}

export default App;
