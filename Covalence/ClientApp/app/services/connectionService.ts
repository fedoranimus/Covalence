import {inject} from 'aurelia-framework';
import {Config} from 'aurelia-api';

@inject(Config)
export class ConnectionService {
    constructor(private config: Config) {
        
    }

    public getConnections(): Promise<IConnection[]> {
        return this.config.getEndpoint('api').find('connection');
    }

    public requestConnection(requestedUserId: string): Promise<IConnection[]> {
        return this.config.getEndpoint('api').post("connection/request", { id: requestedUserId });
    }

    public acceptConnection(requestingUserId: string): Promise<IConnection[]> {
        return this.config.getEndpoint('api').post("connection/approve", { id: requestingUserId });
    }

    public rejectConnection(requestingUserId: string): Promise<IConnection[]> {
        return this.config.getEndpoint('api').post("connection/reject", { id: requestingUserId });
    }

    // public blockConnection(requestingUserId: string): Promise<IConnection[]> {
    //     return this.config.getEndpoint('api').post("connection/block", { id: requestingUserId });
    // }

    public cancelConnection(requestedUserId: string): Promise<IConnection[]> {
        return this.config.getEndpoint('api').post("connection/cancel", { id: requestedUserId });
    }
}

export interface IConnection {
    requestingUserId: string;
    requestedUserId: string;
    displayName: string;
    connectionStatus: ConnectionStatus;
    email?: string;
}

export enum ConnectionStatus {
    requested,
    pending,
    connected,
    blocked,
    available
}