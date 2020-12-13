import React from 'react';

export class SettingsContext {
    context: React.Context<any> = React.createContext(null);

    initialised: boolean | undefined = false;

    async fetchSettings(): Promise<void> {
        fetch('api/settings/getsettings')
            .then((response: { json: () => any; }) => response.json())
            .then(async (data: any[]) => {
                this.context = React.createContext(data);
                this.initialised = true;
            })
            .catch((error: any) => {
                console.log(error);
            })
    }

    static instance(): SettingsContext {
        if (!settingsContext.initialised) {
            settingsContext.fetchSettings();
        }

        return settingsContext;
    }
}

const settingsContext = new SettingsContext();

export default settingsContext;
