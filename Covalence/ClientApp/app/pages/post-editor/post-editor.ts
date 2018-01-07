import { bindable, autoinject } from 'aurelia-framework';
import { PostService } from 'services/postService';
import { PostType } from 'infrastructure/post';
import { Router } from 'aurelia-router';
import { ITag } from "infrastructure/tag";
import { TagService } from "services/tagService";
import { Store } from 'aurelia-store';
import { State } from 'store/state';

@autoinject
export class Post {
    @bindable isEdit: boolean = false;
    @bindable title: string = "";
    @bindable content: string = "";
    @bindable tagList: string[] = [];
    @bindable postId: number = null;
    @bindable postType = PostType.Mentor;

    @bindable suggestedTags: ITag[] = [];

    errorState = null;
    constructor(private postService: PostService, private router: Router, private tagService: TagService, private store: Store<State>) {
        store.state.subscribe(state => {
            
        });
    }

    activate(params) {
        if(params)
            this.postId = params.id;
    }

    async createPost() {
        await this.postService.createPost(this.title, this.content, this.tagList, this.postType);
        this.router.navigateToRoute('home');
    }

    async updatePost() {
        await this.postService.updatePost(this.postId, this.title, this.content, this.tagList, this.postType);
        this.router.navigateToRoute('home');
    }

    async onAddTag(event: CustomEvent) {
        const tagName = event.detail;
        const index = this.tagList.findIndex(x => x == tagName);
        
        if(index === -1) {
            const tag = await this.tagService.getTag(tagName);
            this.tagList.push(tag.name);
        }
    }
}