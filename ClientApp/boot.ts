import 'isomorphic-fetch';
import {Aurelia, PLATFORM} from 'aurelia-framework'
import authConfig from './app/authConfig';
//import grid and reset?
//import 'bootstrap/dist/css/bootstrap.css';
import 'flexboxgrid/dist/flexboxgrid.css';
import 'reset-css/reset.css';
declare const IS_DEV_BUILD: boolean; // The value is supplied by Webpack during the build
declare const localhost = "localhost:5000";

export function configure(aurelia: Aurelia) {
  aurelia.use
    .standardConfiguration()
    .feature('resources')
    .plugin('aurelia-api', config => {
      //config.setDefaultBaseUrl(environment.host);
      config.registerEndpoint('auth');
      config.registerEndpoint('api', localhost + '/api/');
      config.registerEndpoint('me', localhost + '/api/users/me');
      config.registerEndpoint('user', localhost + '/api/users');
      config.setDefaultEndpoint('api');
    })
    .plugin('aurelia-authentication', baseConfig => {
      baseConfig.configure(authConfig);
    });
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

  aurelia.start().then(() => aurelia.setRoot(PLATFORM.moduleName('app/app')));
}