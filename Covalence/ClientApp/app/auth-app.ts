import { autoinject, useView, Aurelia, PLATFORM } from 'aurelia-framework';
import {Router, RouterConfiguration} from 'aurelia-router';
import {AuthenticateStep, AuthService} from 'aurelia-authentication';

@useView('./app.html')
@autoinject
export class AuthApp {
    router: Router;
    
    constructor(private authService: AuthService) {

    }

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = "Covalence";
        config.addAuthorizeStep(AuthenticateStep);
        config.map([
            { route: '', name: 'home', moduleId: PLATFORM.moduleName('./pages/app-home/app-home'), nav: false, title: 'Home' },
            { route: 'profile', name: 'profile', moduleId: PLATFORM.moduleName('./pages/profile/profile'), nav: false, title: 'Profile', auth: true },
            { route: 'logout', name: 'logout', redirect: PLATFORM.moduleName('./pages/home/home'), nav: false, title: 'Logout', auth: true },
            { route: 'post/:id?', name: 'post', moduleId: PLATFORM.moduleName('./pages/post-editor/post-editor'), nav: false, title: 'Post', auth: true }
        ]);
        this.router = router;
    }
}