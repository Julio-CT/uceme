import React from 'react';
import authService from './components/api-authorization/AuthorizeService';

export type Settings = {
  telephone: string;
  address: string;
  contactEmail: string;
  baseHref: string;
};

const defaults: Settings = {
  telephone: '',
  address: '',
  contactEmail: '',
  baseHref: '',
};

const SettingsContext = (): React.Context<Settings> => {
  const defaultSettings: React.Context<Settings> =
    React.createContext(defaults);
  const [context, setContext] =
    React.useState<React.Context<Settings>>(defaultSettings);
  const baseHref: string =
    process.env.NODE_ENV === 'development' ? '' : 'ucemeapi/';

  React.useEffect(() => {
    async function fetchData() {
      const token = await authService.getAccessToken();
      fetch(`${baseHref}api/settings/getsettings`, {
        headers: !token ? {} : { Authorization: `Bearer ${token}` },
      })
        .then((response: Response) => response.json())
        .then(async (data: Settings) => {
          const settings = data;
          if (settings) {
            settings.baseHref = baseHref;
          }

          setContext(React.createContext(settings));
        });
    }
    fetchData();
  }, [baseHref]);

  return context;
};

export default SettingsContext;
