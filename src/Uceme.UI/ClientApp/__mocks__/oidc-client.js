class UserManager {
  constructor(settings) {
    this.settings = settings;
    this.events = {
      addAccessTokenExpiring: () => {},
      addAccessTokenExpired: () => {},
      addUserLoaded: () => {},
      addSilentRenewError: () => {},
      addUserSignedOut: () => {},
    };
  }

  getUser() {
    return Promise.resolve(null);
  }

  signinSilent() {
    return Promise.reject(new Error('signinSilent not implemented in mock'));
  }

  signinPopup() {
    return Promise.reject(new Error('signinPopup not implemented in mock'));
  }

  signinRedirect() {
    return Promise.resolve();
  }

  signoutPopup() {
    return Promise.resolve();
  }

  signoutRedirect() {
    return Promise.resolve();
  }

  removeUser() {
    return Promise.resolve();
  }
}

class WebStorageStateStore {
  constructor(opts) {
    this.prefix = (opts && opts.prefix) || '';
    this._store = {};
  }

  getItem(key) {
    const k = this.prefix + key;
    return this._store.hasOwnProperty(k) ? this._store[k] : null;
  }

  setItem(key, value) {
    const k = this.prefix + key;
    this._store[k] = value;
  }

  removeItem(key) {
    const k = this.prefix + key;
    delete this._store[k];
  }
}

module.exports = {
  UserManager,
  WebStorageStateStore,
};
