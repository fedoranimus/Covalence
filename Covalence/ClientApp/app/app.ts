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
      { route: 'login', name: 'login', moduleId: PLATFORM.moduleName('./pages/login/login'), nav: false, title: 'Login' },
      { route: 'register', name: 'register', moduleId: PLATFORM.moduleName('./pages/register/register'), nav: false, title: 'Sign Up' },
      { route: 'forgot-password', name: 'forgot-password', moduleId: PLATFORM.moduleName("./pages/login/forgot-password/forgot-password"), nav: false, title: 'Forgot Password'  }
    ]);
    this.router = router;
  }
}
