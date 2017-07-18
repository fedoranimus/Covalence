import {autoinject, Aurelia, PLATFORM} from 'aurelia-framework';
import {Router, RouterConfiguration} from 'aurelia-router';
import {AuthenticateStep, AuthService} from 'aurelia-authentication';

@autoinject
export class App {
  router: Router;

  constructor(private authService: AuthService) {

  }

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Covalence";
    config.addAuthorizeStep(AuthenticateStep);
    config.map([
      { route: '', name: 'home', moduleId: PLATFORM.moduleName('./pages/home/home'), nav: false, title: 'Home' },
      { route: 'login', name: 'login', moduleId: PLATFORM.moduleName('./pages/account/account'), nav: false, title: 'Login' },
      { route: 'register', name: 'register', moduleId: PLATFORM.moduleName('./pages/account/account'), nav: false, title: 'Sign Up' }
    ]);
    this.router = router;
  }
}
