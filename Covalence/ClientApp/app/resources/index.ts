import {FrameworkConfiguration, PLATFORM} from 'aurelia-framework';

export function configure(config: FrameworkConfiguration) {
  config.globalResources(
    [
      PLATFORM.moduleName('./custom-elements/tag-editor/tag-editor'),
      PLATFORM.moduleName('./custom-elements/tag-list/tag-list'),
      PLATFORM.moduleName('./custom-elements/result-list/result-list'),
      PLATFORM.moduleName('./custom-elements/user-list/user-list.html'),
      PLATFORM.moduleName('./value-converters/markdown-value-converter')
    ]);
}
