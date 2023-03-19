import { Profile, User, UserManager, WebStorageStateStore } from 'oidc-client';
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

  private userManager?: UserManager;

  constructor() {
    this.ensureUserManagerInitialized();
  }

  async isAuthenticated(): Promise<boolean> {
    const user = await this.getUser();
    return !!user;
  }

  async getUser(): Promise<Profile | null | undefined> {
    if (this.user && this.user.profile) {
      return this.user.profile;
    }

    await this.ensureUserManagerInitialized();
    const user = await this.userManager?.getUser();
    return user && user.profile;
  }

  async getAccessToken(): Promise<string | null | undefined> {
    await this.ensureUserManagerInitialized();
    const user = await this.userManager?.getUser();
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
        if (popUpError.message === 'Popup window closed') {
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
          return AuthorizeService.error(redirectError);
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
        return AuthorizeService.error(redirectSignOutError);
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
      return AuthorizeService.error(error);
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

    const response: Response = await fetch(
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
      await this.userManager?.removeUser();
      this.updateState(undefined);
    });
  }
}

const authService = new AuthorizeService();

export default authService;
