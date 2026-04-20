import { mockFetchSequence, clearFetchMock } from './testUtils';

describe('mockFetchSequence', () => {
  afterEach(() => {
    clearFetchMock();
    jest.restoreAllMocks();
  });

  it('matches using predicate and returns its response', async () => {
    mockFetchSequence([
      {
        predicate: (url: string, opts?: any) =>
          url.includes('/upload') && opts && opts.method === 'POST',
        response: { result: 'uploaded' },
        ok: true,
      },
      { matcher: '/upload', response: { result: 'fallback' }, ok: true },
    ]);

    const resp = await fetch('http://test/api/upload', { method: 'POST' });
    const json = await resp.json();
    expect(json).toEqual({ result: 'uploaded' });
  });

  it('matches by method when matcher is omitted', async () => {
    mockFetchSequence([
      { method: 'POST', response: { ok: 'post' }, ok: true },
      { matcher: '/other', response: { ok: 'other' }, ok: true },
    ]);

    const resp = await fetch('http://test/api/anything', { method: 'POST' });
    const json = await resp.json();
    expect(json).toEqual({ ok: 'post' });
  });

  it('matches by regex matcher', async () => {
    mockFetchSequence([
      { matcher: /\/items\/[0-9]+$/, response: { id: 5 }, ok: true },
    ]);

    const resp = await fetch('http://test/api/items/5');
    const json = await resp.json();
    expect(json).toEqual({ id: 5 });
  });

  it('falls back to last item when nothing matches', async () => {
    mockFetchSequence([
      { matcher: '/nope', response: { a: 1 }, ok: true },
      { matcher: '/also-nope', response: { fallback: true }, ok: true },
    ]);

    const resp = await fetch('http://test/api/unknown');
    const json = await resp.json();
    expect(json).toEqual({ fallback: true });
  });

  it('uses the first matching predicate when multiple predicates are provided', async () => {
    mockFetchSequence([
      {
        predicate: (u) => u.includes('/multi') && u.includes('first'),
        response: { which: 'first' },
        ok: true,
      },
      {
        predicate: (u) => u.includes('/multi'),
        response: { which: 'second' },
        ok: true,
      },
    ]);

    const resp = await fetch('http://test/api/multi?tag=first');
    const json = await resp.json();
    expect(json).toEqual({ which: 'first' });
  });

  it('prefers method+matcher matching correctly when both method and matcher provided', async () => {
    mockFetchSequence([
      {
        matcher: '/resource',
        method: 'POST',
        response: { action: 'created' },
        ok: true,
      },
      { matcher: '/resource', response: { action: 'fetched' }, ok: true },
    ]);

    const postResp = await fetch('http://test/api/resource', {
      method: 'POST',
    });
    expect(await postResp.json()).toEqual({ action: 'created' });

    const getResp = await fetch('http://test/api/resource', { method: 'GET' });
    expect(await getResp.json()).toEqual({ action: 'fetched' });
  });

  it('continues to fallback when a predicate throws', async () => {
    mockFetchSequence([
      {
        predicate: () => {
          throw new Error('boom');
        },
        response: { a: 1 },
        ok: true,
      },
      { matcher: '/safe', response: { ok: true }, ok: true },
    ]);

    const resp = await fetch('http://test/api/safe');
    const json = await resp.json();
    expect(json).toEqual({ ok: true });
  });

  it('matches requests based on custom header value', async () => {
    mockFetchSequence([
      {
        predicate: (_url: string, opts?: any) => {
          const h = opts && opts.headers;
          if (!h) return false;
          // support Headers instance or plain object
          if (typeof h.get === 'function') return h.get('X-Test') === '1';
          return h['X-Test'] === '1' || h['x-test'] === '1';
        },
        response: { headerMatched: true },
        ok: true,
      },
      { matcher: '/header', response: { headerMatched: false }, ok: true },
    ]);

    const resp = await fetch('http://test/api/header', {
      method: 'GET',
      headers: { 'X-Test': '1' },
    });
    const json = await resp.json();
    expect(json).toEqual({ headerMatched: true });
  });

  it('matches requests based on request body content (JSON)', async () => {
    mockFetchSequence([
      {
        predicate: (_url: string, opts?: any) => {
          try {
            if (!opts || !opts.body) return false;
            const parsed = JSON.parse(opts.body);
            return parsed && parsed.name === 'john';
          } catch (e) {
            return false;
          }
        },
        response: { bodyMatched: true },
        ok: true,
      },
      { matcher: '/body', response: { bodyMatched: false }, ok: true },
    ]);

    const resp = await fetch('http://test/api/body', {
      method: 'POST',
      body: JSON.stringify({ name: 'john' }),
      headers: { 'Content-Type': 'application/json' },
    });
    const json = await resp.json();
    expect(json).toEqual({ bodyMatched: true });
  });

  it('selects the first matching item when multiple items match the same criteria', async () => {
    mockFetchSequence([
      { matcher: '/order', response: { which: 1 }, ok: true },
      { matcher: '/order', response: { which: 2 }, ok: true },
    ]);

    const resp = await fetch('http://test/api/order');
    const json = await resp.json();
    expect(json).toEqual({ which: 1 });
  });
});
