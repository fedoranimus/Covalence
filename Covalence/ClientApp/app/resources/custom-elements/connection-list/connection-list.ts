import { autoinject } from 'aurelia-framework';
import { bindable } from 'aurelia-framework';
import { ConnectionService, IConnection, ConnectionStatus } from 'services/connectionService';
import { Store } from 'aurelia-store';
import { State } from 'store/state';
import { loadConnections, updateConnection } from 'store/connectionActions';

@autoinject
export class ConnectionList {
    @bindable connections: IConnection[];

    activeFilter: ConnectionStatus | null = null;

    constructor(private store: Store<State>, private connectionService: ConnectionService) {

    }

    get hasConnections() {
        if(this.activeFilter) {
            return this.connections.filter(c => c.connectionStatus === this.activeFilter).length > 0;
        }
        return this.connections.length > 0;
    }

    created() {
        this.store.dispatch(loadConnections, () => this.connectionService.getConnections());
    }    

    filter(status: ConnectionStatus | null) {
        this.activeFilter = status;
    }

    filterFunc(searchExpression, connection: IConnection) {
        if((searchExpression === null || searchExpression === undefined) || !connection) return false;

        return connection.connectionStatus == searchExpression;
    }

    async confirmConnection(userId: string) {
        this.store.dispatch(updateConnection, userId, ConnectionStatus.connected, (userId) => this.connectionService.acceptConnection(userId));
    }

    async rejectConnection(userId: string) {
        this.store.dispatch(updateConnection, userId, ConnectionStatus.available, (userId) => this.connectionService.rejectConnection(userId));
    }

    async cancelConnectionRequest(userId: string) {
        this.store.dispatch(updateConnection, userId, ConnectionStatus.available, (userId) => this.connectionService.cancelConnection(userId));
    }
}