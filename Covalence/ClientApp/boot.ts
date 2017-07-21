import 'isomorphic-fetch';
import { Aurelia, PLATFORM } from 'aurelia-framework'
import authConfig from './app/authConfig';
import { AuthService } from 'aurelia-authentication';
import 'bulma/css/bulma.css';
declare const IS_DEV_BUILD: boolean; // The value is supplied by Webpack during the build

export function configure(aurelia: Aurelia) {
  aurelia.use
    .standardConfiguration()
    .feature(PLATFORM.moduleName('app/resources/index'))
    .plugin(PLATFORM.moduleName('aurelia-api'), config => {
      //config.setDefaultBaseUrl(environment.host);
      config.registerEndpoint('auth', 'http://localhost:5000/');
      config.registerEndpoint('api', 'http://localhost:5000/api/');
      config.setDefaultEndpoint('api');
    })
    .plugin(PLATFORM.moduleName('aurelia-authentication/authenticatedFilterValueConverter'))
    .plugin(PLATFORM.moduleName('aurelia-authentication/authenticatedValueConverter'))
    .plugin(PLATFORM.moduleName('aurelia-authentication/authFilterValueConverter'))
    .plugin(PLATFORM.moduleName('aurelia-authentication'), baseConfig => {
      baseConfig.configure(authConfig);
    })
    .plugin(PLATFORM.moduleName('aurelia-validation'));
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

  aurelia.start().then(() => {
    const auth = aurelia.container.get(AuthService);
    if(auth.isAuthenticated())
      aurelia.setRoot(PLATFORM.moduleName('app/auth-app'));
    else
      aurelia.setRoot(PLATFORM.moduleName('app/app'));
  });
}