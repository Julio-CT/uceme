/* eslint-disable import/no-extraneous-dependencies */
// Compatibility re-export: utilities have been moved to the test-only folder
// so imports that expect `src/testUtils/testUtils.tsx` keep working while
// ESLint rules about devDependencies remain satisfied.
import * as React from 'react';
import { render, RenderResult } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import SettingsContext, { Settings } from '../SettingsContext';
import authService from '../components/api-authorization/AuthorizeService';

// Types exported for discoverability
export type RenderResultWithSettings = RenderResult;

export function renderWithSettings(
  ui: React.ReactElement,
  settings?: Partial<Settings>
): RenderResultWithSettings {
  const value: Settings = {
    baseHref: 'http://localhost/',
    ...(settings || {}),
  } as Settings;
  return render(
    <SettingsContext.Provider value={value}>{ui}</SettingsContext.Provider>
  );
}

// Fetch mock utilities
export type FetchPredicate = (url: string, opts?: any) => boolean;

export type FetchSequenceItem = {
  matcher?: RegExp | string; // url pattern to match (regex or substring). Optional when using `predicate`.
  method?: string; // optional HTTP method to match (e.g. 'GET', 'POST')
  predicate?: FetchPredicate; // optional predicate function (url, opts) => boolean
  response: any; // value returned by response.json()
  ok?: boolean; // response.ok
  status?: number; // HTTP status
};

type FetchMock = (url: string, opts?: any) => Promise<any>;

/**
 * Install a fetch mock that selects the response based on the first matching
 * `matcher` (RegExp or substring). If none matches, uses the last item as fallback.
 */
export function mockFetchSequence(sequence: FetchSequenceItem[]): void {
  // @ts-ignore
  global.fetch = jest.fn((url: string, opts?: any) => {
    // find first item where predicate(url, opts) === true OR method matches (if provided) and matcher matches
    const found =
      sequence.find((s) => {
        try {
          if (s.predicate) {
            return s.predicate(url, opts);
          }

          if (s.method && opts && opts.method) {
            if (
              String(s.method).toUpperCase() !==
              String(opts.method).toUpperCase()
            )
              return false;
          }

          if (s.matcher === undefined || s.matcher === null) {
            // if no matcher provided, and no predicate, treat it as match-only-by-method
            return !!s.method;
          }

          if (s.matcher instanceof RegExp)
            return (s.matcher as RegExp).test(url);
          return String(url).includes(String(s.matcher));
        } catch (e) {
          return false;
        }
      }) || sequence[sequence.length - 1];

    const ok = found.ok !== undefined ? found.ok : true;
    let status: number;
    if (found.status !== undefined) {
      status = found.status as number;
    } else {
      status = ok ? 200 : 500;
    }
    return Promise.resolve({
      ok,
      status,
      json: () => Promise.resolve(found.response),
    });
  }) as unknown as FetchMock;
}

export function mockAuthToken(token: string | null = 'token'): void {
  jest.spyOn(authService, 'getAccessToken').mockResolvedValue(token as any);
}

export function clearFetchMock(): void {
  try {
    // @ts-ignore
    delete global.fetch;
  } catch (e) {
    // ignore
  }
}

// user-event wrapper for convenience
export const user = userEvent;

// Note: no default export to encourage named imports. Utilities are
// intentionally exported as named symbols only.
