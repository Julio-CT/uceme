import React from 'react';

export type Settings = {
  telephone: string;
  address: string;
  contactEmail: string;
  baseHref: string;
} | null;

const SettingsContext = (): React.Context<Settings> => {
  const defaultSettings: React.Context<any> = React.createContext(null);
  const [context, setContext] = React.useState<React.Context<Settings>>(
    defaultSettings
  );
  const baseHref: string =
    process.env.NODE_ENV === 'development' ? '' : 'ucemeapi/';

  React.useEffect(() => {
    fetch(`${baseHref}api/settings/getsettings`)
      .then((response: { json: () => any }) => response.json())
      .then(async (data: Settings) => {
        if (data) {
          data.baseHref = baseHref;
        }

        setContext(React.createContext(data));
      })
      .catch((error: any) => {
        console.log(error);
      });
  }, [baseHref]);

  return context;
};

export default SettingsContext;
