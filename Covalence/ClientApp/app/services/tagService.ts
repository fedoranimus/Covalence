import {inject} from 'aurelia-framework';
import {Config} from 'aurelia-api';
import {ITag} from '../infrastructure/tag';

@inject(Config)
export class TagService {
    constructor(private config: Config) {
        
    }

    public getAllTags(): Promise<string[]> {
        return this.config.getEndpoint('api').find('tag');
    }

    public getTag(tagName: string): Promise<ITag> {
        return this.config.getEndpoint('api').findOne('tag', tagName);
    }
}