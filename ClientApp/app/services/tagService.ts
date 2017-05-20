import {inject} from 'aurelia-framework';
import {Config} from 'aurelia-api';

@inject(Config)
export class TagService {
    constructor(private config: Config) {
        
    }

    public getAllTags(): Promise<string[]> {
        return this.config.getEndpoint('api').find('tags');
    }
}