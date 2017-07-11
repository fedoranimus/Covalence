import { IPost, PostType } from '../infrastructure/post';
import { autoinject } from 'aurelia-framework';
import { Config } from 'aurelia-api';

@autoinject
export class PostService {
    constructor(private config: Config) {

    }

    async getAllPosts(): Promise<IPost[]> {
        return await this.config.getEndpoint('api').find('post');
    }

    async createPost(title: string, content: string, tagList: string[], postType: PostType): Promise<IPost> {
        const post = { title: title, content: content, tags: tagList, category: postType };
        return await this.config.getEndpoint('api').create('post', post);
    }

    async updatePost(postId: number, title: string, content: string, tagList: string[], postType: PostType): Promise<IPost> {
        const post = { title: title, content: content, tags: tagList, category: postType };
        return await this.config.getEndpoint('api').updateOne('post', postId, null, post);
    }
}