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
        return this.config.getEndpoint('api').post("request", requestedUserId);
    }

    public acceptConnection(requestingUserId: string): Promise<IConnectionList> {
        return this.config.getEndpoint('api').post("approve", requestingUserId);
    }

    public rejectConnection(requestingUserId: string): Promise<IConnectionList> {
        return this.config.getEndpoint('api').post("reject", requestingUserId);
    }

    public blockConnection(requestingUserId: string): Promise<IConnectionList> {
        return this.config.getEndpoint('api').post("block", requestingUserId);
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