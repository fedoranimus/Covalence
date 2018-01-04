import { PagedList } from 'services/searchService';
import { ConnectionService, IConnectionList } from 'services/connectionService';
import { Router } from 'aurelia-router';
import {AuthService} from 'aurelia-authentication';
import {autoinject, computedFrom} from 'aurelia-framework';
import { PostService } from 'services/postService';
import { ITag } from 'infrastructure/tag';
import { IPost } from 'infrastructure/post'; 
import { IUser } from 'infrastructure/user';
import { SearchService } from 'services/searchService';

@autoinject
export class AppHome {
    public tags: ITag[] = [];
    connections: IConnectionList;

    constructor(private authService: AuthService, private router: Router, private searchService: SearchService, private connectionService: ConnectionService) {
        
    }

    async bind() {
        if(this.authenticated) { //check if user is logged in
            try {
                let user = await this.authService.getMe();

                this.connections = await this.connectionService.getConnections();
                this.tags = user.tags;
                console.debug("Authenticated", user, this.connections);
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