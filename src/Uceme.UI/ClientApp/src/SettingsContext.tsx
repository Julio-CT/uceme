import React from 'react';

export type Settings = {
    telephone: string;
    address: string;
    contactEmail: string;
  } | null;

const SettingsContextAlt = (): React.Context<Settings> => {
    const defaultSettings: React.Context<any> = React.createContext(null);
    const [context, setContext] = React.useState<React.Context<Settings>>(defaultSettings);

    React.useEffect(() => {
        fetch('api/settings/getsettings')
            .then((response: { json: () => any; }) => response.json())
            .then(async (data: Settings) => {
                setContext(React.createContext(data));
            })
            .catch((error: any) => {
                console.log(error);
            })    
        }, []);

    return context;
}

export default SettingsContextAlt;
