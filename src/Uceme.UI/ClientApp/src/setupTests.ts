// jest-dom adds custom jest matchers for asserting on DOM nodes.
// allows you to do things like:
// expect(element).toHaveTextContent(/react/i)
// learn more: https://github.com/testing-library/jest-dom
import '@testing-library/jest-dom/extend-expect';
import 'jest-dom/extend-expect';

/// /const localStorageMock = {
/// /    getItem: jest.fn(),
/// /    setItem: jest.fn(),
/// /    removeItem: jest.fn(),
/// /    clear: jest.fn(),
/// /};

/// /(global as any).localStorage = localStorageMock;

// Mock the request issued by the react app to get the client configuration parameters.
/// /window.fetch = () => {
/// /  return Promise.resolve(
/// /    {
/// /      ok: true,
/// /      json: () => Promise.resolve({
/// /        "authority": "https://localhost:5001",
/// /        "client_id": "Uceme.UI",
/// /        "redirect_uri": "https://localhost:5001/authentication/login-callback",
/// /        "post_logout_redirect_uri": "https://localhost:5001/authentication/logout-callback",
/// /        "response_type": "id_token token",
/// /        "scope": "Uceme.UIAPI openid profile"
/// /      }),
/// /    headers: new Headers(),
/// /    redirected: false,
/// /    status: 1,
/// /    statusText: "",
/// /    trailer: new Promise<Headers>(void),
/// /    type: null,
/// /    url: null,
/// /    clone: null,
/// /    body: null,
/// /    bodyUsed: null,
/// /    blob: null,
/// /    text: null,
/// /    arrayBuffer: null,
/// /    formData: null
/// /    });
/// /};
