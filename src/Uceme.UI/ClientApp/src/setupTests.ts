import Enzyme from 'enzyme';
import ReactSeventeenAdapter from '@wojtekmaj/enzyme-adapter-react-17';

Enzyme.configure({ adapter: new ReactSeventeenAdapter() });

const localStorageMock = {
  getItem: jest.fn(),
  setItem: jest.fn(),
  removeItem: jest.fn(),
  clear: jest.fn(),
};

(global as any).localStorage = localStorageMock;

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
