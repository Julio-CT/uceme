/* eslint-disable @typescript-eslint/no-explicit-any */
import type { Profile, User } from 'oidc-client';
import ApplicationPaths, {
  ApplicationName,
  Arguments,
  Callback,
  ResultModel,
  ResultState,
} from './ApiAuthorizationConstants';

export const AuthenticationResultStatus = {
  Redirect: 'redirect',
  Success: 'success',
  Fail: 'fail',
};

export class AuthorizeService {
  private callbacks = Array<Callback>();

  private nextSubscriptionId = 0;

  private user: User | null | undefined = null;

  // By default pop ups are disabled because they don't work properly on Edge.
  // If you want to enable pop up authentication simply set this flag to false.
  private popUpDisabled = true;

  // Runtime-managed object from oidc-client; keep as any to avoid importing
  // the concrete type at module evaluation time and to prevent type errors
  // when dynamically importing the module.
  private userManager?: any;

  // Do not initialize the user manager in the constructor to avoid
  // triggering oidc-client usage (which may access browser APIs) at import time.
  // Initialization will happen lazily when methods that need it are called.

  async isAuthenticated(): Promise<boolean> {
    await this.ensureUserManagerInitialized();
    const user = await this.userManager?.getUser();
    if (!user) {
      return false;
    }

    // Check if token is expired or about to expire (within 60 seconds)
    const expiresAt = user.expires_at;
    if (expiresAt) {
      const now = Math.floor(Date.now() / 1000);
      const timeUntilExpiry = expiresAt - now;
      // If token is expired or expires within 60 seconds, try to refresh
      if (timeUntilExpiry <= 60) {
        try {
          const refreshedUser = await this.userManager?.signinSilent();
          if (refreshedUser) {
            this.updateState(refreshedUser);
            return true;
          }
        } catch (error) {
          // Silent renewal failed, user needs to re-authenticate
          this.updateState(null);
          return false;
        }
      }
    }

    return true;
  }

  async getUser(): Promise<Profile | null | undefined> {
    await this.ensureUserManagerInitialized();
    const user = await this.getValidUser();
    return user && user.profile;
  }

  private async getValidUser(): Promise<User | null | undefined> {
    await this.ensureUserManagerInitialized();
    const user = await this.userManager?.getUser();

    if (!user) {
      return null;
    }

    // Check if token is expired or about to expire (within 60 seconds)
    const expiresAt = user.expires_at;
    if (expiresAt) {
      const now = Math.floor(Date.now() / 1000);
      const timeUntilExpiry = expiresAt - now;
      // If token is expired or expires within 60 seconds, try to refresh
      if (timeUntilExpiry <= 60) {
        try {
          const refreshedUser = await this.userManager?.signinSilent();
          if (refreshedUser) {
            this.updateState(refreshedUser);
            return refreshedUser;
          }
        } catch (error) {
          // Silent renewal failed, clear user state
          this.updateState(null);
          return null;
        }
      }
    }

    return user;
  }

  async getAccessToken(): Promise<string | null | undefined> {
    await this.ensureUserManagerInitialized();
    const user = await this.getValidUser();
    return user && user.access_token;
  }

  // We try to authenticate the user in three different ways:
  // 1) We try to see if we can authenticate the user silently. This happens
  //    when the user is already logged in on the IdP and is done using a hidden iframe
  //    on the client.
  // 2) We try to authenticate the user using a PopUp Window. This might fail if there is a
  //    Pop-Up blocker or the user has disabled PopUps.
  // 3) If the two methods above fail, we redirect the browser to the IdP to perform a traditional
  //    redirect flow.
  async signIn(state: ResultState): Promise<ResultModel> {
    await this.ensureUserManagerInitialized();
    try {
      const silentUser: User | undefined = await this.userManager?.signinSilent(
        AuthorizeService.createArguments(null)
      );
      this.updateState(silentUser);
      return AuthorizeService.success(state);
    } catch (silentError) {
      try {
        if (this.popUpDisabled) {
          throw new Error(
            "Popup disabled. Change 'AuthorizeService.js:AuthorizeService._popupDisabled' to false to enable it."
          );
        }

        const popUpUser: User | undefined = await this.userManager?.signinPopup(
          AuthorizeService.createArguments(null)
        );
        this.updateState(popUpUser);
        return AuthorizeService.success(state);
      } catch (popUpError) {
        if ((popUpError as Error).message === 'Popup window closed') {
          // The user explicitly cancelled the login action by closing an opened popup.
          return AuthorizeService.error('The user closed the window.');
        }

        // PopUps might be blocked by the user, fallback to redirect
        try {
          await this.userManager?.signinRedirect(
            AuthorizeService.createArguments(state)
          );
          return AuthorizeService.redirect();
        } catch (redirectError) {
          return AuthorizeService.error((redirectError as Error).message);
        }
      }
    }
  }

  async completeSignIn(url: string): Promise<ResultModel> {
    try {
      await this.ensureUserManagerInitialized();
      const user = await this.userManager?.signinCallback(url);
      this.updateState(user);
      return AuthorizeService.success(user && user.state);
    } catch (error) {
      return AuthorizeService.error('There was an error signing in.');
    }
  }

  // We try to sign out the user in two different ways:
  // 1) We try to do a sign-out using a PopUp Window. This might fail if there is a
  //    Pop-Up blocker or the user has disabled PopUps.
  // 2) If the method above fails, we redirect the browser to the IdP to perform a traditional
  //    post logout redirect flow.
  async signOut(state: ResultState): Promise<ResultModel> {
    await this.ensureUserManagerInitialized();
    try {
      if (this.popUpDisabled) {
        throw new Error(
          "Popup disabled. Change 'AuthorizeService.js:AuthorizeService._popupDisabled' to false to enable it."
        );
      }

      await this.userManager?.signoutPopup(
        AuthorizeService.createArguments(null)
      );
      this.updateState(undefined);
      return AuthorizeService.success(state);
    } catch (popupSignOutError) {
      try {
        await this.userManager?.signoutRedirect(
          AuthorizeService.createArguments(state)
        );
        return AuthorizeService.redirect();
      } catch (redirectSignOutError) {
        return AuthorizeService.error((redirectSignOutError as Error).message);
      }
    }
  }

  async completeSignOut(url: string): Promise<ResultModel> {
    await this.ensureUserManagerInitialized();
    try {
      const response = await this.userManager?.signoutCallback(url);
      this.updateState(null);
      return AuthorizeService.success(response && response.state);
    } catch (error) {
      return AuthorizeService.error((error as Error).message);
    }
  }

  updateState(user: User | null | undefined): void {
    this.user = user;
    this.notifySubscribers();
  }

  subscribe(callback: () => Promise<void>): number {
    this.callbacks.push({
      callback,
      subscription: this.nextSubscriptionId,
    });
    this.nextSubscriptionId += 1;
    return this.nextSubscriptionId - 1;
  }

  unsubscribe(subscriptionId: number, caller: string): void {
    const subscriptionIndex = this.callbacks
      .map((element, index) =>
        element.subscription === subscriptionId
          ? { found: true, index }
          : { found: false }
      )
      .filter((element) => element.found === true);
    if (subscriptionIndex.length !== 1) {
      throw new Error(`Unable to unsusbcribe for ${caller}`);
    }

    const callbackIndex: number = subscriptionIndex[0].index as number;
    this.callbacks.splice(callbackIndex, 1);
  }

  notifySubscribers(): void {
    for (let i = 0; i < this.callbacks.length; i += 1) {
      const { callback } = this.callbacks[i];
      callback();
    }
  }

  static createArguments(state: ResultState | null): Arguments {
    return { useReplaceToNavigate: true, data: state };
  }

  static error(message: string): ResultModel {
    return { status: AuthenticationResultStatus.Fail, message, state: null };
  }

  static success(state: ResultState): ResultModel {
    return { status: AuthenticationResultStatus.Success, state, message: null };
  }

  static redirect(): ResultModel {
    return {
      status: AuthenticationResultStatus.Redirect,
      message: null,
      state: null,
    };
  }

  async ensureUserManagerInitialized(): Promise<void> {
    if (this.userManager !== undefined) {
      return;
    }
    // Lazy import to avoid touching browser APIs at module import time
    // (which can trigger SecurityError in test environments).
    // Importing inside the method ensures it happens at runtime when needed.
    const oidcModule = await import('oidc-client');
    // Some module systems expose the exports as default; support both shapes
    // by checking for `.default` and falling back to the namespace itself.
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const oidc: any = oidcModule && (oidcModule.default || oidcModule);
    const response: Response = await fetch(
      ApplicationPaths.ApiAuthorizationClientConfigurationUrl
    );

    if (!response.ok) {
      throw new Error(`Could not load settings for '${ApplicationName}'`);
    }

    const settings = await response.json();
    // Enable automatic silent renewal to refresh tokens before they expire
    settings.automaticSilentRenew = true;
    // Set access token expiration time to renew 60 seconds before expiry
    settings.accessTokenExpiringNotificationTime = 60;
    settings.monitorSession = false;
    settings.includeIdTokenInSilentRenew = true;
    settings.userStore = new oidc.WebStorageStateStore({
      prefix: ApplicationName,
    });

    this.userManager = new oidc.UserManager(settings);

    // Handle token renewal events
    // Note: When automaticSilentRenew is enabled, the library handles renewal automatically
    // These event handlers are for notification and state management
    this.userManager.events.addAccessTokenExpiring(() => {
      // Token is about to expire, automatic renewal should be triggered
      // We just need to ensure state is updated when renewal completes
    });

    this.userManager.events.addAccessTokenExpired(() => {
      // Token has expired, try to renew it manually as a fallback
      this.userManager
        ?.signinSilent()
        .then((user: any) => {
          if (user) {
            this.updateState(user);
          } else {
            this.updateState(null);
          }
        })
        .catch(() => {
          // Silent renewal failed, clear user state
          this.updateState(null);
        });
    });

    this.userManager.events.addUserLoaded((user: any) => {
      // User was loaded (including after token renewal)
      this.updateState(user);
    });

    this.userManager.events.addSilentRenewError(() => {
      // Silent renewal failed, user needs to re-authenticate
      this.updateState(null);
    });

    this.userManager.events.addUserSignedOut(async () => {
      await this.userManager?.removeUser();
      this.updateState(undefined);
    });
  }
}

const authService = new AuthorizeService();

export default authService;
