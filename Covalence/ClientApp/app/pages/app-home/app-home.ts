import { PagedList } from 'services/searchService';
import { ConnectionService } from 'services/connectionService';
import { Router } from 'aurelia-router';
import { AuthService } from 'aurelia-authentication';
import { autoinject, computedFrom } from 'aurelia-framework';
import { PostService } from 'services/postService';
import { ITag } from 'infrastructure/tag';
import { IPost } from 'infrastructure/post'; 
import { IUser } from 'infrastructure/user';
import { SearchService } from 'services/searchService';
import { Store } from 'aurelia-store';
import { State } from 'store/state';
import { getCurrentUser } from 'store/userActions';
import { search } from 'store/searchActions';

@autoinject
export class AppHome {
    private state: State;
    // TODO - create subscription array 

    constructor(private authService: AuthService, private router: Router, private searchService: SearchService, private connectionService: ConnectionService, private store: Store<State>) {
        store.state.subscribe(state => {
            this.state = state;
        });
    }

    async search() {
        const searchQuery = this.state.searchQuery;
        this.store.dispatch(search, searchQuery, null, (searchQuery) => this.searchService.getResults(searchQuery, null));
    }

    @computedFrom('authService.authenticated')
    get authenticated() {
        return this.authService.authenticated;
    }

    deactivate() {
        //this.subscriptions.foreach(sub => sub.unsubscribe()):
    }
}