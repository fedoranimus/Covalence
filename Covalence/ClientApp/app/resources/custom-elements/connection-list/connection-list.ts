import { autoinject } from 'aurelia-framework';
import { bindable } from 'aurelia-framework';
import { IConnectionList, ConnectionService } from 'services/connectionService';
import { Store } from 'aurelia-store';
import { State } from 'store/state';
import { loadConnections } from 'store/connectionActions';

@autoinject
export class ConnectionList {
    @bindable connections: IConnectionList;

    constructor(private store: Store<State>, private connectionService: ConnectionService) {

    }

    created() {
        this.store.dispatch(loadConnections, () => this.connectionService.getConnections());
    }    
}