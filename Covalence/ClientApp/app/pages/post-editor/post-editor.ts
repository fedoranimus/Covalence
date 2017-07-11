import { bindable, autoinject } from 'aurelia-framework';
import { PostService } from '../../services/postService';
import { PostType } from '../../infrastructure/post';
import { Router } from 'aurelia-router';

@autoinject
export class Post {
    @bindable isEdit: boolean = false;
    @bindable title: string = "";
    @bindable content: string = "";
    @bindable tagList: string[] = [];
    @bindable postId: number = null;
    @bindable postType = PostType.Mentor;
    constructor(private postService: PostService, private router: Router) {

    }

    async createPost() {
        await this.postService.createPost(this.title, this.content, this.tagList, this.postType);
        this.router.navigateToRoute('home');
    }

    async updatePost() {
        await this.postService.updatePost(this.postId, this.title, this.content, this.tagList, this.postType);
        this.router.navigateToRoute('home');
    }
}