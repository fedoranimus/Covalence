import {AuthService} from 'aurelia-authentication';
import {autoinject, computedFrom} from 'aurelia-framework';
import { PostService } from '../../services/postService';
import { ITag } from '../../infrastructure/tag';
import { IPost } from '../../infrastructure/post'; 

@autoinject
export class AppHome {
    public tags: ITag[] = [];
    public posts: IPost[] = [];

    constructor(private authService: AuthService, private postService: PostService) {
        
    }

    async bind() {
        if(this.authenticated) { //check if user is logged in
            try {
                let user = await this.authService.getMe();
                this.tags = user.tags;
                console.debug("Authenticated", user);

                this.posts = await this.postService.getAllPosts();
            } catch(e) {
                //console.error(e);
            }
        }
    }



    @computedFrom('authService.authenticated')
    get authenticated() {
        return this.authService.authenticated;
    }
}