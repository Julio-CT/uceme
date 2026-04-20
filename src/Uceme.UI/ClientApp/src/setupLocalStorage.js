// Jest setup file that provides a basic localStorage mock before modules initialize
const localStorageMock = {
  getItem: jest.fn(() => null),
  setItem: jest.fn(() => undefined),
  removeItem: jest.fn(() => undefined),
  clear: jest.fn(() => undefined),
};

global.localStorage = localStorageMock;

try {
  Object.defineProperty(window, 'localStorage', {
    value: localStorageMock,
    configurable: true,
    writable: true,
  });
} catch (e) {
  // ignore in case of restricted environment
}

module.exports = {};
