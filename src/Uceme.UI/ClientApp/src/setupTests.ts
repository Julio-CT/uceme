// Provide a safe in-memory localStorage/sessionStorage for tests.
// This prevents `SecurityError: localStorage is not available for opaque origins`
// when jsdom runs tests with an opaque origin.
if (typeof global !== 'undefined' && !(global as any).localStorage) {
  const _store: Record<string, string> = {};
  const storage = {
    getItem: (key: string) => (_store.hasOwnProperty(key) ? _store[key] : null),
    setItem: (key: string, value: string) => {
      _store[key] = String(value);
    },
    removeItem: (key: string) => {
      delete _store[key];
    },
    clear: () => {
      Object.keys(_store).forEach((k) => delete _store[k]);
    },
  } as Storage;

  (global as any).localStorage = storage;
  (global as any).sessionStorage = storage;

  if (typeof window !== 'undefined' && !('localStorage' in window)) {
    Object.defineProperty(window, 'localStorage', {
      configurable: true,
      enumerable: true,
      value: storage,
    });
    Object.defineProperty(window, 'sessionStorage', {
      configurable: true,
      enumerable: true,
      value: storage,
    });
  }
}

// Diagnostic: check jsdom environment and whether localStorage is accessible.
try {
  // eslint-disable-next-line no-console
  console.log('setupTests: window.location.href=', typeof window !== 'undefined' && window.location ? window.location.href : 'no-window');
  try {
    // Try a harmless access to localStorage
    const testLS = (window as any).localStorage ? (window as any).localStorage.getItem('___test') : null;
    // eslint-disable-next-line no-console
    console.log('setupTests: localStorage access ok, value=', testLS);
  } catch (e) {
    // eslint-disable-next-line no-console
    console.error('setupTests: localStorage access threw', e && e.stack ? e.stack : e);
  }
} catch (e) {
  // ignore
}

// Mock the request issued by the react app to get the client configuration parameters.
window.fetch = () => {
  return Promise.resolve({
    ok: true,
    json: () =>
      Promise.resolve({
        authority: 'https://localhost:5001',
        client_id: 'Uceme.UI',
        redirect_uri: 'https://localhost:5001/authentication/login-callback',
        post_logout_redirect_uri:
          'https://localhost:5001/authentication/logout-callback',
        response_type: 'id_token token',
        scope: 'Uceme.UIAPI openid profile',
      }),
    headers: new Headers(),
    redirected: false,
    status: 1,
    statusText: '',
    trailer: null,
    type: null,
    url: null,
    clone: null,
    body: null,
    bodyUsed: null,
    blob: null,
    text: null,
    arrayBuffer: null,
    formData: null,
  } as unknown as Response);
};

export {};
