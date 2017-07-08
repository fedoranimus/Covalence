import { bindable, autoinject } from 'aurelia-framework';
import { IPost } from '../../../infrastructure/post';

export class PostList {
    @bindable posts: IPost[];
    constructor() {

    }
}