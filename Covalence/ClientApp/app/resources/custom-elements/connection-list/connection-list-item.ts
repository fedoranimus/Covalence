import { IRemoteUser } from 'infrastructure/user';
import { autoinject } from 'aurelia-framework';
import { State } from 'store/state';
import { Store } from 'aurelia-store';
import { ConnectionService, ConnectionStatus, IConnection } from 'services/connectionService';
import { StringUtils } from 'utils/stringUtils';
import { rejectConnection, confirmConnection, cancelConnection, disconnectConnection } from 'store/connectionActions';
import { bindable } from 'aurelia-templating';

@autoinject
export class ConnectionListItem {
    @bindable connection: IConnection;
    private copyBtn: Element;

    constructor(private store: Store<State>, private connectionService: ConnectionService) {

    }

    copyEmail(email: string) {
        StringUtils.copyText(email);
        this.copyBtn.innerHTML = "Copied!";
        setTimeout(() => {
            this.copyBtn.innerHTML = "Copy";
        }, 3000);
    }

    async confirmConnection(userId: string) {
        this.store.dispatch(confirmConnection, userId);
    }

    async rejectConnection(userId: string) {
        this.store.dispatch(rejectConnection, userId);
    }

    async cancelConnectionRequest(userId: string) {
        this.store.dispatch(cancelConnection, userId);
    }

    async disconnectConnection(userId: string) {
        this.store.dispatch(disconnectConnection, userId);
    }
}