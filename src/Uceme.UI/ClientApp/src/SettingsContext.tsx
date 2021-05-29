import React from 'react';

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
    fetch(`${baseHref}api/settings/getsettings`)
      .then((response: Response) => response.json())
      .then(async (data: Settings) => {
        const settings = data;
        if (settings) {
          settings.baseHref = baseHref;
        }

        setContext(React.createContext(settings));
      });
  }, [baseHref]);

  return context;
};

export default SettingsContext;
