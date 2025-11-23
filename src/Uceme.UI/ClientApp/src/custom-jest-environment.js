const JsdomEnvironment = require('jest-environment-jsdom');

class CustomJestEnvironment extends JsdomEnvironment {
  constructor(config, context) {
    super(config, context);

    // Provide a safe localStorage for tests in case jsdom runs with an
    // opaque origin or localStorage is not available.
    try {
      if (!this.global.localStorage) {
        const store = {};
        this.global.localStorage = {
          getItem: (key) => (Object.prototype.hasOwnProperty.call(store, key) ? store[key] : null),
          setItem: (key, value) => {
            store[key] = String(value);
          },
          removeItem: (key) => {
            delete store[key];
          },
          clear: () => {
            Object.keys(store).forEach((k) => delete store[k]);
          },
        };
      }

      if (this.global.window && !this.global.window.localStorage) {
        Object.defineProperty(this.global.window, 'localStorage', {
          configurable: true,
          enumerable: true,
          value: this.global.localStorage,
        });
      }
    } catch (e) {
      // If anything goes wrong setting localStorage, swallow the error so the
      // environment still initializes — tests will likely fail later with
      // clearer messages.
    }
  }
}

module.exports = CustomJestEnvironment;
