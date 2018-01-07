import {autoinject, Aurelia, PLATFORM} from 'aurelia-framework';
import {Router, RouterConfiguration} from 'aurelia-router';
import {AuthenticateStep, AuthService} from 'aurelia-authentication';
import { Store } from 'aurelia-store';
import { State } from 'store/state';
import { getCurrentUser } from 'store/userActions';

@autoinject
export class App {
  router: Router;

  constructor(private authService: AuthService, private store: Store<State>) {
    this.registerActions();
  }

  private registerActions() {
    this.store.registerAction(getCurrentUser.name, getCurrentUser);
  }

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Covalence";
    config.addAuthorizeStep(AuthenticateStep);
    config.map([
      { route: '', name: 'home', moduleId: PLATFORM.moduleName('./pages/home/home'), nav: false, title: 'Home' },
      { route: 'login', name: 'login', moduleId: PLATFORM.moduleName('./pages/login/login'), nav: false, title: 'Login' },
      { route: 'register/:email?', name: 'register', moduleId: PLATFORM.moduleName('./pages/register/register'), nav: false, title: 'Sign Up' },
      { route: 'forgot-password', name: 'forgot-password', moduleId: PLATFORM.moduleName('./pages/login/forgot-password/forgot-password'), nav: false, title: 'Forgot Password'  }
    ]).mapUnknownRoutes(PLATFORM.moduleName('./pages/not-found'));
    this.router = router;
  }
}
