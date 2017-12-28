import { bindable } from 'aurelia-framework';
import { IConnectionList } from './../../../services/connectionService';
export class ConnectionList {

    @bindable connections: IConnectionList;

    constructor() {

    }

    async bind() {
        console.log(this.connections);
    }

    attached() {
        console.log(this.connections);
    }

    
}