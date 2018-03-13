import { ConnectionService } from 'services/connectionService';
import { State } from 'store/state';
import { ConnectionStatus, IConnection } from 'services/connectionService';
import { Container } from 'aurelia-framework';
import { connect } from 'http2';

const connectionService = Container.instance.get(ConnectionService) as ConnectionService;

export async function loadConnections(state: State, getConnectionList: Function) {
    const connections = await getConnectionList();

    return { ...state, ...{ connections } };
}

export function requestConnection(state: State, userId: string) {
    return updateConnection(state, userId, ConnectionStatus.requested, (userId) => connectionService.requestConnection(userId));
}

export function confirmConnection(state: State, userId: string) {
    return updateConnection(state, userId, ConnectionStatus.connected, (userId) => connectionService.acceptConnection(userId));
}

export function rejectConnection(state: State, userId: string) {
    return updateConnection(state, userId, ConnectionStatus.available, (userId) => connectionService.rejectConnection(userId));
}

export function cancelConnection(state: State, userId: string) {
    return updateConnection(state, userId, ConnectionStatus.available, (userId) => connectionService.cancelConnection(userId));
}

export function disconnectConnection(state: State, userId: string) {
    return updateConnection(state, userId, ConnectionStatus.available, (userId) => connectionService.rejectConnection(userId));
}

async function updateConnection(state: State, userId: string, newConnectionStatus: ConnectionStatus, connectionApi: (userId: string) => Promise<IConnection[]>) {
    try {
        const connections = await connectionApi(userId);
        if(state.remoteUserDetails) {
            const remoteUserDetails = state.remoteUserDetails;
            remoteUserDetails.connectionStatus = newConnectionStatus;
            return { ...state, ...remoteUserDetails };
        } else {
            const user = state.results.items.find(u => u.id == userId);
            if(user) {
                user.connectionStatus = newConnectionStatus;
            
                const index = state.results.items.findIndex(u => u.id == userId);
            
                return { ...state, ...{ 
                        results: {
                            ...state.results,
                            items: [
                                ...state.results.items.slice(0, index),
                                Object.assign({}, user),
                                ...state.results.items.slice(index + 1)
                            ]
                        },
                        connections: connections 
                    } 
                };
            }
        }
        
        return { ...state, ...{ connections }};
    } catch(e) {
        console.error(e);
        return { ...state };
    }
}