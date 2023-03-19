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
  baseHref: process.env.NODE_ENV === 'development' ? '' : 'ucemeapi/',
};

const SettingsContext: React.Context<Settings> = React.createContext(defaults);

export default SettingsContext;
