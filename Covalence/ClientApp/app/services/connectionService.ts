import { IConnectionList } from './connectionService';
import {inject} from 'aurelia-framework';
import {Config} from 'aurelia-api';

@inject(Config)
export class ConnectionService {
    constructor(private config: Config) {
        
    }

    public getConnections(): Promise<IConnectionList> {
        return this.config.getEndpoint('api').find('connection');
    }

    public requestConnection(requestedUserId: string): Promise<IConnectionList> {
        return this.config.getEndpoint('api').post("connection/request", { id: requestedUserId });
    }

    public acceptConnection(requestingUserId: string): Promise<IConnectionList> {
        return this.config.getEndpoint('api').post("connection/approve", { id: requestingUserId });
    }

    public rejectConnection(requestingUserId: string): Promise<IConnectionList> {
        return this.config.getEndpoint('api').post("connection/reject", { id: requestingUserId });
    }

    public blockConnection(requestingUserId: string): Promise<IConnectionList> {
        return this.config.getEndpoint('api').post("connection/block", { id: requestingUserId });
    }

    public cancelConnection(requestedUserId: string): Promise<IConnectionList> {
        return this.config.getEndpoint('api').post("connection/cancel", { id: requestedUserId });
    }
}

export interface IConnectionList {
    requestedConnections: IConnection[];
    activeConnections: IConnection[];
    pendingConnections: IConnection[];
}

export interface IConnection {
    requestingUserId: string;
    requestedUserId: string;
    displayName: string;
}

export enum ConnectionStatus {
    requested,
    pending,
    connected,
    blocked,
    available
}