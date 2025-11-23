const { JSDOM } = require('jsdom');

class EarlyJestEnvironment {
  constructor(config) {
    this.config = config || {};

    // Create a JSDOM instance early with a non-opaque URL so localStorage
    // and other web APIs are available and won't throw SecurityError.
    const dom = new JSDOM('<!doctype html><html><body></body></html>', {
      url: (this.config.testEnvironmentOptions && this.config.testEnvironmentOptions.url) || 'http://localhost',
      runScripts: 'dangerously',
      resources: 'usable',
    });

    const win = dom.window;

    // Provide a safe localStorage/sessionStorage if jsdom's default throws.
    try {
      if (!win.localStorage || (function () {
        try {
          // try simple access
          win.localStorage.getItem('__jest_test__');
          return false;
        } catch (e) {
          return true;
        }
      })()) {
        const _store = {};
        win.localStorage = {
          getItem: (k) => (_store.hasOwnProperty(k) ? _store[k] : null),
          setItem: (k, v) => { _store[k] = String(v); },
          removeItem: (k) => { delete _store[k]; },
          clear: () => { Object.keys(_store).forEach((k) => delete _store[k]); },
        };
        win.sessionStorage = win.localStorage;
      }
    } catch (e) {
      // ignore
    }

    // Copy necessary properties to the Node global so modules see a browser-like env
    const exposedProperties = ['window', 'document', 'navigator', 'localStorage', 'sessionStorage', 'location', 'history', 'Event', 'CustomEvent', 'getComputedStyle'];
    exposedProperties.forEach((prop) => {
      if (prop in win) {
        global[prop] = win[prop];
      }
    });

    // Keep reference for teardown
    this.dom = dom;
    this.win = win;
    // Jest expects the environment instance to expose a `global` object.
    this.global = win;
    // Provide a moduleMocker so Jest can create mocks in this environment.
    try {
      // eslint-disable-next-line global-require
      const { ModuleMocker } = require('jest-mock');
      this.moduleMocker = new ModuleMocker(this.global);
    } catch (e) {
      // ignore; some jest setups provide moduleMocker differently
    }
    // Mirror some Node globals into the JSDOM window so modules that expect
    // `process`, `console`, or timers work as in Jest's default env.
    try {
      this.global.process = typeof process !== 'undefined' ? process : undefined;
      this.global.console = typeof console !== 'undefined' ? console : this.global.console;
      this.global.setTimeout = setTimeout;
      this.global.clearTimeout = clearTimeout;
      this.global.setInterval = setInterval;
      this.global.clearInterval = clearInterval;
    } catch (e) {
      // ignore
    }
    try {
      // Ensure `global` is defined inside the vm context like Node's global.
      this.global.global = this.global;
    } catch (e) {
      // ignore
    }
  }

  async setup() {
    // Nothing special here; globals already set in constructor
    return Promise.resolve();
  }

  async teardown() {
    try {
      if (this.dom && this.dom.window) this.dom.window.close();
    } catch (e) {
      // ignore
    }
    return Promise.resolve();
  }

  // Jest >=27 requires environments to provide a VM context.
  getVmContext() {
    try {
      if (this.dom && typeof this.dom.getInternalVMContext === 'function') {
        return this.dom.getInternalVMContext();
      }
    } catch (e) {
      // ignore
    }
    return null;
  }
}

module.exports = EarlyJestEnvironment;
