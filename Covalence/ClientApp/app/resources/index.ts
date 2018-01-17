import {FrameworkConfiguration, PLATFORM} from 'aurelia-framework';

export function configure(config: FrameworkConfiguration) {
  config.globalResources(
    [
      PLATFORM.moduleName('./custom-elements/tag-editor/tag-editor'),
      PLATFORM.moduleName('./custom-elements/search-bar/search-bar'),
      PLATFORM.moduleName('./custom-elements/user-list/user-list'),
      PLATFORM.moduleName('./custom-elements/connection-list/connection-list'),
      PLATFORM.moduleName('./custom-elements/banner/banner.html'),
      PLATFORM.moduleName('./value-converters/markdown-value-converter'),
      PLATFORM.moduleName('./value-converters/connection-state-value-converter'),
      PLATFORM.moduleName('./value-converters/filter-value-converter')
    ]);
}
