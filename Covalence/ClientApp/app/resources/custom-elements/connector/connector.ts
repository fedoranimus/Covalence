import { autoinject, bindable } from 'aurelia-framework';
import { IRemoteUser } from 'infrastructure/user';
import { ConnectionStatus } from 'services/connectionService';
import { requestConnection, confirmConnection, rejectConnection, cancelConnection, disconnectConnection } from 'store/connectionActions';
import { State } from 'store/state';
import { Store } from 'aurelia-store';

@autoinject
export class Connector {
    @bindable remoteUser: IRemoteUser;
    emailConfirmed: boolean = false;

    constructor(private store: Store<State>) {
        this.store.state.subscribe(state => {
            if(state.user)
                this.emailConfirmed = state.user.emailConfirmed;
        });
    }

    async request(userId: string) {
        this.store.dispatch(requestConnection, userId);
    }

    async confirm(userId: string) {
        this.store.dispatch(confirmConnection, userId);
    }

    async reject(userId: string) {
        this.store.dispatch(rejectConnection, userId);
    }

    async disconnect(userId: string) {
        this.store.dispatch(disconnectConnection, userId);
    }

    async cancelRequest(userId: string) {
        this.store.dispatch(cancelConnection, userId);
    }
}