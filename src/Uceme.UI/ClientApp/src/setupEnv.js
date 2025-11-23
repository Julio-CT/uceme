// Minimal early test environment setup (plain JS so Node can require it before transforms)
// Purpose: provide a lightweight in-memory `localStorage`/`sessionStorage` and
// a minimal `window` placeholder so modules that access storage at import time
// don't hit jsdom's opaque-origin SecurityError before the test environment
// is initialized by Jest.
/* eslint-disable no-console */
(function () {
  try {
    if (typeof global !== 'undefined' && !global.localStorage) {
      var _store = Object.create(null);
      var storage = {
        getItem: function (key) {
          return Object.prototype.hasOwnProperty.call(_store, key) ? _store[key] : null;
        },
        setItem: function (key, value) {
          _store[key] = String(value);
        },
        removeItem: function (key) {
          delete _store[key];
        },
        clear: function () {
          Object.keys(_store).forEach(function (k) { delete _store[k]; });
        }
      };

      global.localStorage = storage;
      global.sessionStorage = storage;

      if (typeof global.window === 'undefined') {
        // Lightweight placeholder; real jsdom will replace this during Jest env setup.
        global.window = { localStorage: storage, sessionStorage: storage };
      } else if (!('localStorage' in global.window)) {
        try {
          Object.defineProperty(global.window, 'localStorage', {
            configurable: true,
            enumerable: true,
            value: storage,
          });
          Object.defineProperty(global.window, 'sessionStorage', {
            configurable: true,
            enumerable: true,
            value: storage,
          });
        } catch (e) {
          // ignore if we cannot define properties
        }
      }
    }
  } catch (e) {
    // Swallow any errors in this minimal polyfill — it's only diagnostic/defensive.
  }
})();
