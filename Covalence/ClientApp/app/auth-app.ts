import { IUser } from 'infrastructure/user';
import { UserService } from 'services/userService';
import { autoinject, useView, Aurelia, PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration, NavigationInstruction, Next, Redirect, RedirectToRoute } from 'aurelia-router';
import { AuthenticateStep, AuthService } from 'aurelia-authentication';
import { Store } from 'aurelia-store';
import { State } from 'store/state';
import { loadConnections, requestConnection, confirmConnection, cancelConnection, disconnectConnection, rejectConnection } from 'store/connectionActions';
import { search, navigateToPage, addSearchParam, removeSearchParam, clearSearchParams } from 'store/searchActions';
import { getCurrentUser, completeOnboarding, clearUser, updateCurrentUser, clearCurrentUser, getUser } from 'store/userActions';

@useView('./app.html')
@autoinject
export class AuthApp {
    private state: State;
    
    constructor(private authService: AuthService, private store: Store<State>, private app: Aurelia, private router: Router) {
        this.registerActions();
        this.store.state.subscribe(state => {
            this.state = state;
        });
    }

    async created() {
        if(this.authService.authenticated) {
            await this.store.dispatch(getCurrentUser, () => this.authService.getMe());
        }
    }

    private registerActions() {
        this.registerUserActions();
        this.registerSearchActions();
        this.registerConnectionActions();
    }

    private registerUserActions() {
        this.store.registerAction(getUser.name, getUser);
        this.store.registerAction(getCurrentUser.name, getCurrentUser);
        this.store.registerAction(completeOnboarding.name, completeOnboarding);
        this.store.registerAction(clearUser.name, clearUser);
        this.store.registerAction(clearCurrentUser.name, clearCurrentUser);
        this.store.registerAction(updateCurrentUser.name, updateCurrentUser);
    }

    private registerSearchActions() {
        this.store.registerAction(search.name, search);
        this.store.registerAction(navigateToPage.name, navigateToPage);
        this.store.registerAction(addSearchParam.name, addSearchParam);
        this.store.registerAction(removeSearchParam.name, removeSearchParam);
        this.store.registerAction(clearSearchParams.name, clearSearchParams);
    }

    private registerConnectionActions() {
        this.store.registerAction(loadConnections.name, loadConnections);
        this.store.registerAction(confirmConnection.name, confirmConnection);
        this.store.registerAction(cancelConnection.name, cancelConnection);
        this.store.registerAction(disconnectConnection.name, disconnectConnection);
        this.store.registerAction(rejectConnection.name, rejectConnection);
        this.store.registerAction(requestConnection.name, requestConnection);
    }

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = "Covalence";
        config.addAuthorizeStep(AuthenticateStep);
        config.addPreActivateStep(CheckOnboarding);
        config.map([
            { route: '', name: 'home', moduleId: PLATFORM.moduleName('./pages/app-home/app-home'), nav: false, title: 'Home' },
            { route: 'settings', name: 'settings', moduleId: PLATFORM.moduleName('./pages/settings/settings'), nav: false, title: 'Settings', auth: true },
            { route: 'profile/:id', name: 'profile', moduleId: PLATFORM.moduleName('./pages/profile/profile'), nav: false, title: 'Profile', auth: true },
            { route: 'logout', name: 'logout', redirect: PLATFORM.moduleName('./pages/home/home'), nav: false, title: 'Logout', auth: true },
            // { route: 'post/:id?', name: 'post', moduleId: PLATFORM.moduleName('./pages/post-editor/post-editor'), nav: false, title: 'Post', auth: true },
            { route: 'onboard', name: 'onboard', moduleId: PLATFORM.moduleName('./pages/register/onboarding/onboarding'), nav: false, title: 'Welcome to Covalence', auth: true }
        ]).mapUnknownRoutes(PLATFORM.moduleName('./pages/not-found'));
        this.router = router;
    }
}

@autoinject
export class CheckOnboarding {
    constructor(private authService: AuthService) {
    }

    async run(navigationInstruction: NavigationInstruction, next: Next): Promise<any> {
        if(this.authService.authenticated) {
            const currentUser = await this.authService.getMe();
            if(!currentUser || navigationInstruction.config.route === 'onboard')
                return next();

            if(currentUser.needsOnboarding)
                return next.cancel(new RedirectToRoute('onboard'));  
            else 
                return next();
        } else {
            return next();
        }
        
    }
}