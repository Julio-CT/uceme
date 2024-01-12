import * as React from 'react';
import { ReactElement } from 'react';
import { Route, Routes } from 'react-router';
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
import ScheduleManager from './components/admin/ScheduleManager';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import ApplicationPaths, {
  LoginActions,
  LogoutActions,
} from './components/api-authorization/ApiAuthorizationConstants';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import SettingsContext, { Settings } from './SettingsContext';
import Technique from './components/Technique';
import './App.scss';
import './custom.scss';
import AboutUs from './components/AboutUs';
import Login from './components/api-authorization/Login';
import Logout from './components/api-authorization/Logout';

function loginAction(name: string): ReactElement {
  return <Login action={name} />;
}

function logoutAction(name: string): ReactElement {
  return <Logout action={name} />;
}

function App(): ReactElement {
  const [context, setContext] = React.useState<Settings>({
    telephone: 'cargando...',
    address: 'cargando...',
    contactEmail: 'cargando...',
    baseHref: process.env.NODE_ENV === 'development' ? '' : 'ucemeapi/',
  });

  const defaults = React.useMemo<Settings>(
    () => ({
      telephone: 'cargando...',
      address: 'cargando...',
      contactEmail: 'cargando...',
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
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/condiciones" element={<Conditions />} />
          <Route path="/especialidades" element={<Specialities />} />
          <Route path="/quienessomos" element={<AboutUs />} />
          <Route path="/innovaciones" element={<Home />} />
          <Route path="/blog/:page?" element={<BlogHome />} />
          <Route path="/post/:slug" element={<BlogItem />} />
          <Route path="/especialidad/:esp" element={<Speciality />} />
          <Route path="/tecnica/:tec" element={<Technique />} />
          <Route path="/contacto" element={<ContactUs />} />
          <Route
            path="/appointmentmanager"
            element={<AuthorizeRoute element={<AppointmentManager />} />}
          />
          <Route
            path="/postmanager"
            element={<AuthorizeRoute element={<PostManager />} />}
          />
          <Route
            path="/schedulemanager"
            element={<AuthorizeRoute element={<ScheduleManager />} />}
          />
          <Route
            path={ApplicationPaths.ApiAuthorizationPrefix}
            element={loginAction(LoginActions.Login)}
          />
          <Route
            path={ApplicationPaths.Login}
            element={loginAction(LoginActions.Login)}
          />
          <Route
            path={ApplicationPaths.LoginFailed}
            element={loginAction(LoginActions.LoginFailed)}
          />
          <Route
            path={ApplicationPaths.LoginCallback}
            element={loginAction(LoginActions.LoginCallback)}
          />
          <Route
            path={ApplicationPaths.Profile}
            element={loginAction(LoginActions.Profile)}
          />
          <Route
            path={ApplicationPaths.Register}
            element={loginAction(LoginActions.Register)}
          />
          <Route
            path={ApplicationPaths.LogOut}
            element={logoutAction(LogoutActions.Logout)}
          />
          <Route
            path={ApplicationPaths.LogOutCallback}
            element={logoutAction(LogoutActions.LogoutCallback)}
          />
          <Route
            path={ApplicationPaths.LoggedOut}
            element={logoutAction(LogoutActions.LoggedOut)}
          />
          <Route path="/adminlogin" element={<ApiAuthorizationRoutes />} />
        </Routes>
        <div />
      </Layout>
    </SettingsContext.Provider>
  );
}

export default App;
