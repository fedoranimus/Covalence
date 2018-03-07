import { IRemoteUser } from './../../../infrastructure/user';
import { computedFrom } from 'aurelia-binding';
import { bindable, autoinject } from "aurelia-framework";
import { ConnectionService, ConnectionStatus } from "services/connectionService";
import { State } from "store/state";
import { Store } from "aurelia-store";
import { IUser } from "infrastructure/user";
import { updateConnection } from "store/connectionActions";

@autoinject
export class UserListItem {
    @bindable user: IRemoteUser;
    @bindable currentUser: IUser;
    
    constructor(private connectionService: ConnectionService, private store: Store<State>) {

    }

    @computedFrom('user.distanceToUser')
    get hasDistance() {
        return !(isNaN(this.user.distanceToUser));
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
}