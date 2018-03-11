import { IRemoteUser } from 'infrastructure/user';
import { NavigationInstruction, RouteConfig } from 'aurelia-router';
import { Store } from 'aurelia-store';
import { State } from 'store/state';
import { UserService } from 'services/userService';
import { getUser, clearUser } from 'store/userActions';
import { autoinject } from 'aurelia-framework';
import { updateConnection } from 'store/connectionActions';
import { ConnectionStatus, ConnectionService } from 'services/connectionService';

@autoinject
export class Profile {
    remoteUserDetails: IRemoteUser;
    isEmailConfirmed: boolean = false;

    constructor(private store: Store<State>, private userService: UserService, private connectionService: ConnectionService) {
        this.store.state.subscribe(state => {
            this.remoteUserDetails = state.remoteUserDetails;
            if(state.user)
                this.isEmailConfirmed = state.user.emailConfirmed;
        });
    }

    activate(params: any) {
        this.store.dispatch(getUser, params.id, (id) => this.userService.getUser(params.id));   
    }

    async requestConnection(userId: string) {
        try {
            this.store.dispatch(updateConnection, userId, ConnectionStatus.requested, (userId) => this.connectionService.requestConnection(userId));
        } catch(e) {
            console.error(e);
        }
    }

    async confirmConnection(userId: string) {
        try {
            this.store.dispatch(updateConnection, userId, ConnectionStatus.connected, (userId) => this.connectionService.acceptConnection(userId));
        } catch(e) {
            console.error(e);
        }
    }

    async rejectConnection(userId: string) {
        try {
            this.store.dispatch(updateConnection, userId, ConnectionStatus.available, (userId) => this.connectionService.rejectConnection(userId));
        } catch(e) {
            console.error(e);
        }
    }

    async cancelConnectionRequest(userId: string) {
        try {
            this.store.dispatch(updateConnection, userId, ConnectionStatus.available, (userId) => this.connectionService.cancelConnection(userId));
        } catch(e) {
            console.error(e);
        }
    }

    unbind() {
        this.store.dispatch(clearUser);
    }
}