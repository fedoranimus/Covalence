import { IUser } from './infrastructure/user';
import { UserService } from './services/userService';
import { autoinject, useView, Aurelia, PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration, NavigationInstruction, Next, Redirect, RedirectToRoute } from 'aurelia-router';
import { AuthenticateStep, AuthService } from 'aurelia-authentication';

@useView('./app.html')
@autoinject
export class AuthApp {
    router: Router;
    
    constructor(private authService: AuthService) {

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
        ]).mapUnknownRoutes(PLATFORM.moduleName('./pages/not-found.html'));
        this.router = router;
    }
}

@autoinject
export class CheckOnboarding {

    constructor(private userService: UserService) {

    }

    run(navigationInstruction: NavigationInstruction, next: Next): Promise<any> {
        console.log("Preactivate");
        const user = this.userService.currentUser;
        if(!user || navigationInstruction.config.route === 'onboard')
            return next();

        if(user.needsOnboarding)
            return next.cancel(new RedirectToRoute('onboard'));  
        else 
            return next();
    }
}