import 'isomorphic-fetch';
import { Aurelia, PLATFORM } from 'aurelia-framework'
import authConfig from './app/authConfig';
import { AuthService } from 'aurelia-authentication';
import 'bulma/css/bulma.css';
import 'bulma-tooltip/bulma-tooltip.min.css';
import { initialState } from 'store/state';
declare const IS_DEV_BUILD: boolean; // The value is supplied by Webpack during the build

export function configure(aurelia: Aurelia) {
  aurelia.use
    .standardConfiguration()
    .feature(PLATFORM.moduleName('app/resources/index'))
    .plugin(PLATFORM.moduleName('aurelia-api'), config => {
      //config.setDefaultBaseUrl(environment.host);
      if(!IS_DEV_BUILD) { // TODO: Refactor
        config.registerEndpoint('auth', 'https://covalence.timdturner.com/');
        config.registerEndpoint('api', 'https://covalence.timdturner.com/api/');
      } else {
        config.registerEndpoint('auth', 'http://localhost:5000/');
        config.registerEndpoint('api', 'http://localhost:5000/api/');
      }

      config.setDefaultEndpoint('api');
    })
    .plugin(PLATFORM.moduleName('aurelia-authentication/authenticatedFilterValueConverter'))
    .plugin(PLATFORM.moduleName('aurelia-authentication/authenticatedValueConverter'))
    .plugin(PLATFORM.moduleName('aurelia-authentication/authFilterValueConverter'))
    .plugin(PLATFORM.moduleName('aurelia-authentication'), baseConfig => {
      baseConfig.configure(authConfig);
    })
    .plugin(PLATFORM.moduleName('aurelia-validation'))
    .plugin(PLATFORM.moduleName('aurelia-store'), { initialState });
    // .plugin('aurelia-google-maps', config => {
    //   config.options({
    //     apiKey: 'AIzaSyCEeOKnNEsBO4T4Nm-54p_l4hOaGAx7U_c',
    //     apiLibraries: 'geocoding',
    //     options: {}
    //   });
    // });

  if (IS_DEV_BUILD) {
    aurelia.use.developmentLogging();
  }

  aurelia.start().then(async () => {
    const auth = aurelia.container.get(AuthService);
    let user = null;
    try {
      user = await auth.getMe(); // I really hate this check to ensure the token is correct
    } finally {
      if(!user && auth.isAuthenticated()) {
        auth.logout();
      }

      if(auth.isAuthenticated())
        aurelia.setRoot(PLATFORM.moduleName('app/auth-app'));
      else
        aurelia.setRoot(PLATFORM.moduleName('app/app'));
    }
  });
}