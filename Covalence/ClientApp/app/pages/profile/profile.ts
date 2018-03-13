import { IRemoteUser } from 'infrastructure/user';
import { NavigationInstruction, RouteConfig } from 'aurelia-router';
import { Store } from 'aurelia-store';
import { State } from 'store/state';
import { UserService } from 'services/userService';
import { getUser, clearUser } from 'store/userActions';
import { autoinject } from 'aurelia-framework';

@autoinject
export class Profile {
    remoteUserDetails: IRemoteUser;

    constructor(private store: Store<State>, private userService: UserService) {
        this.store.state.subscribe(state => {
            this.remoteUserDetails = state.remoteUserDetails;
        });
    }

    activate(params: any) {
        this.store.dispatch(getUser, params.id, (id) => this.userService.getUser(params.id));   
    }

    unbind() {
        this.store.dispatch(clearUser);
    }
}