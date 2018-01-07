import { IUser } from 'infrastructure/user';
import { UserService } from 'services/userService';
import { autoinject, useView, Aurelia, PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration, NavigationInstruction, Next, Redirect, RedirectToRoute } from 'aurelia-router';
import { AuthenticateStep, AuthService } from 'aurelia-authentication';
import { Store } from 'aurelia-store';
import { State } from 'store/state';
import { loadConnections, acceptConnection, updateConnection } from 'store/connectionActions';
import { getAll, search, navigateToPage } from 'store/searchActions';
import { getCurrentUser } from 'store/userActions';

@useView('./app.html')
@autoinject
export class AuthApp {
    router: Router;
    
    constructor(private authService: AuthService, private store: Store<State>) {
        this.registerActions();
    }

    async bind() {
        // TODO - refresh logged in user
    }

    private registerActions() {
        this.store.registerAction(loadConnections.name, loadConnections);
        this.store.registerAction(updateConnection.name, updateConnection);
        this.store.registerAction(getAll.name, getAll);
        this.store.registerAction(search.name, search);
        this.store.registerAction(navigateToPage.name, navigateToPage);
        this.store.registerAction(getCurrentUser.name, getCurrentUser);
    }

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = "Covalence";
        config.addAuthorizeStep(AuthenticateStep);
        config.addPreActivateStep(CheckOnboarding);
        config.map([
            { route: '', name: 'home', moduleId: PLATFORM.moduleName('./pages/app-home/app-home'), nav: false, title: 'Home' },
            { route: 'profile', name: 'profile', moduleId: PLATFORM.moduleName('./pages/profile/profile'), nav: false, title: 'Profile', auth: true },
            { route: 'logout', name: 'logout', redirect: PLATFORM.moduleName('./pages/home/home'), nav: false, title: 'Logout', auth: true },
            { route: 'post/:id?', name: 'post', moduleId: PLATFORM.moduleName('./pages/post-editor/post-editor'), nav: false, title: 'Post', auth: true },
            { route: 'onboard', name: 'onboard', moduleId: PLATFORM.moduleName('./pages/register/onboarding/onboarding'), nav: false, title: 'Welcome to Covalence', auth: true }
        ]).mapUnknownRoutes(PLATFORM.moduleName('./pages/not-found'));
        this.router = router;
    }
}

@autoinject
export class CheckOnboarding {
    private currentUser: IUser | null = null;
    constructor(private store:Store<State>) {
        store.state.subscribe(state => {
            this.currentUser = state.user;
        });
    }

    run(navigationInstruction: NavigationInstruction, next: Next): Promise<any> {
        if(!this.currentUser || navigationInstruction.config.route === 'onboard')
            return next();

        if(this.currentUser.needsOnboarding)
            return next.cancel(new RedirectToRoute('onboard'));  
        else 
            return next();
    }
}