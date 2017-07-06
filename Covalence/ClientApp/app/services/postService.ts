import { IPost } from '../infrastructure/post';
import { autoinject } from 'aurelia-framework';
import { Config } from 'aurelia-api';

@autoinject
export class PostService {
    constructor(private config: Config) {

    }

    async getAllPosts(): Promise<IPost[]> {
        return await this.config.getEndpoint('api').find('post');
    }
}