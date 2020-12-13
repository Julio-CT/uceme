import { UserManager, WebStorageStateStore } from 'oidc-client';
import ApplicationPaths, { ApplicationName } from './ApiAuthorizationConstants';

type ResultModel = {
  message: any;
  state: any;
  status: any;
};

export const AuthenticationResultStatus = {
  Redirect: 'redirect',
  Success: 'success',
  Fail: 'fail',
};

export class AuthorizeService {
  private callbacks = Array<any>();

  private nextSubscriptionId = 0;

  private user: any = null;

  // By default pop ups are disabled because they don't work properly on Edge.
  // If you want to enable pop up authentication simply set this flag to false.
  private popUpDisabled = true;

  private userManager: any;

  async isAuthenticated(): Promise<boolean> {
    const user = await this.getUser();
    return !!user;
  }

  async getUser(): Promise<any> {
    if (this.user && this.user.profile) {
      return this.user.profile;
    }

    await this.ensureUserManagerInitialized();
    const user = await this.userManager.getUser();
    return user && user.profile;
  }

  async getAccessToken(): Promise<any> {
    await this.ensureUserManagerInitialized();
    const user = await this.userManager.getUser();
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
  async signIn(state: any): Promise<ResultModel> {
    await this.ensureUserManagerInitialized();
    try {
      const silentUser = await this.userManager.signinSilent(
        this.createArguments(null)
      );
      this.updateState(silentUser);
      return this.success(state);
    } catch (silentError) {
      // User might not be authenticated, fallback to popup authentication
      console.log('Silent authentication error: ', silentError);

      try {
        if (this.popUpDisabled) {
          throw new Error(
            "Popup disabled. Change 'AuthorizeService.js:AuthorizeService._popupDisabled' to false to enable it."
          );
        }

        const popUpUser = await this.userManager.signinPopup(
          this.createArguments(null)
        );
        this.updateState(popUpUser);
        return this.success(state);
      } catch (popUpError) {
        if (popUpError.message === 'Popup window closed') {
          // The user explicitly cancelled the login action by closing an opened popup.
          return this.error('The user closed the window.');
        }
        if (!this.popUpDisabled) {
          console.log('Popup authentication error: ', popUpError);
        }

        // PopUps might be blocked by the user, fallback to redirect
        try {
          await this.userManager.signinRedirect(this.createArguments(state));
          return this.redirect();
        } catch (redirectError) {
          console.log('Redirect authentication error: ', redirectError);
          return this.error(redirectError);
        }
      }
    }
  }

  async completeSignIn(url: any): Promise<ResultModel> {
    try {
      await this.ensureUserManagerInitialized();
      const user = await this.userManager.signinCallback(url);
      this.updateState(user);
      return this.success(user && user.state);
    } catch (error) {
      console.log('There was an error signing in: ', error);
      return this.error('There was an error signing in.');
    }
  }

  // We try to sign out the user in two different ways:
  // 1) We try to do a sign-out using a PopUp Window. This might fail if there is a
  //    Pop-Up blocker or the user has disabled PopUps.
  // 2) If the method above fails, we redirect the browser to the IdP to perform a traditional
  //    post logout redirect flow.
  async signOut(state: any): Promise<ResultModel> {
    await this.ensureUserManagerInitialized();
    try {
      if (this.popUpDisabled) {
        throw new Error(
          "Popup disabled. Change 'AuthorizeService.js:AuthorizeService._popupDisabled' to false to enable it."
        );
      }

      await this.userManager.signoutPopup(this.createArguments(null));
      this.updateState(undefined);
      return this.success(state);
    } catch (popupSignOutError) {
      console.log('Popup signout error: ', popupSignOutError);
      try {
        await this.userManager.signoutRedirect(this.createArguments(state));
        return this.redirect();
      } catch (redirectSignOutError) {
        console.log('Redirect signout error: ', redirectSignOutError);
        return this.error(redirectSignOutError);
      }
    }
  }

  async completeSignOut(url: any): Promise<ResultModel> {
    await this.ensureUserManagerInitialized();
    try {
      const response = await this.userManager.signoutCallback(url);
      this.updateState(null);
      return this.success(response && response.data);
    } catch (error) {
      console.log(`There was an error trying to log out '${error}'.`);
      return this.error(error);
    }
  }

  updateState(user: any): void {
    this.user = user;
    this.notifySubscribers();
  }

  subscribe(callback: any): number {
    this.callbacks.push({
      callback,
      subscription: this.nextSubscriptionId,
    });
    return this.nextSubscriptionId - 1;
  }

  unsubscribe(subscriptionId: any): void {
    const subscriptionIndex = this.callbacks
      .map((element, index) =>
        element.subscription === subscriptionId
          ? { found: true, index }
          : { found: false }
      )
      .filter((element) => element.found === true);
    if (subscriptionIndex.length !== 1) {
      throw new Error(
        `Found an invalid number of subscriptions ${subscriptionIndex.length}`
      );
    }

    const tempShit: number = subscriptionIndex[0].index as number;
    this.callbacks.splice(tempShit, 1);
  }

  notifySubscribers(): void {
    for (let i = 0; i < this.callbacks.length; i += 1) {
      const { callback } = this.callbacks[i];
      callback();
    }
  }

  createArguments(state: any): any {
    return { useReplaceToNavigate: true, data: state };
  }

  error(message: any): ResultModel {
    return { status: AuthenticationResultStatus.Fail, message, state: null };
  }

  success(state: any): ResultModel {
    return { status: AuthenticationResultStatus.Success, state, message: null };
  }

  redirect(): ResultModel {
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

    const response = await fetch(
      ApplicationPaths.ApiAuthorizationClientConfigurationUrl
    );
    if (!response.ok) {
      throw new Error(`Could not load settings for '${ApplicationName}'`);
    }

    const settings = await response.json();
    settings.automaticSilentRenew = true;
    settings.includeIdTokenInSilentRenew = true;
    settings.userStore = new WebStorageStateStore({
      prefix: ApplicationName,
    });

    this.userManager = new UserManager(settings);

    this.userManager.events.addUserSignedOut(async () => {
      await this.userManager.removeUser();
      this.updateState(undefined);
    });
  }

  static get instance(): AuthorizeService {
    return authService;
  }
}

const authService = new AuthorizeService();

export default authService;
