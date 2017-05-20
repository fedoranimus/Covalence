import {inject} from 'aurelia-framework';
import {Config} from 'aurelia-api';
import {ITag} from '../infrastructure/user';

@inject(Config)
export class UserService {
    constructor(private config: Config) {
        
    }

    public addTag(tag: string, tagType: "study"|"expert"): Promise<ITag[]> {
        return this.config.getEndpoint('api').create('users/me/tags', {
            tagType: tagType,
            tagName: tag
        });
    }

    public removeTag(tag: string, tagType: "study"|"expert"): Promise<void> {
        return this.config.getEndpoint('api').destroy('users/me/tags', {
            tagType: tagType,
            tagName: tag
        })
    }

    public getUser(userId: string): Promise<void> {
        return this.config.getEndpoint('api').findOne('users', userId);
    }
}