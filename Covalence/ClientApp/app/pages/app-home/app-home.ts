import { Router } from 'aurelia-router';
import {AuthService} from 'aurelia-authentication';
import {autoinject, computedFrom} from 'aurelia-framework';
import { PostService } from '../../services/postService';
import { ITag } from '../../infrastructure/tag';
import { IPost } from '../../infrastructure/post'; 

@autoinject
export class AppHome {
    public tags: ITag[] = [];

    constructor(private authService: AuthService, private router: Router) {
        
    }

    async bind() {
        if(this.authenticated) { //check if user is logged in
            try {
                let user = await this.authService.getMe();

                if(user.needsOnboarding)
                    this.router.navigate('onboard', { replace: true, trigger: false });

                this.tags = user.tags;
                console.debug("Authenticated", user);
            } catch(e) {
                console.error(e);
            }
        }
    }



    @computedFrom('authService.authenticated')
    get authenticated() {
        return this.authService.authenticated;
    }
}