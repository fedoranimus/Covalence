import { autoinject } from 'aurelia-framework';
import { bindable } from 'aurelia-framework';
import { ConnectionService, IConnection, ConnectionStatus } from 'services/connectionService';
import { Store } from 'aurelia-store';
import { State } from 'store/state';
import { loadConnections } from 'store/connectionActions';

@autoinject
export class ConnectionList {
    @bindable connections: IConnection[];

    activeFilter: ConnectionStatus | null = null;

    constructor(private store: Store<State>, private connectionService: ConnectionService) {

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
}