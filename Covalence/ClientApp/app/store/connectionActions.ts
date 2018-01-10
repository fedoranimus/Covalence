import { State } from 'store/state';
import { ConnectionStatus, IConnection } from 'services/connectionService';

export async function loadConnections(state: State, getConnectionList: Function) {
    const connections = await getConnectionList();

    return { ...state, ...{ connections } };
}

export async function updateConnection(state: State, userId: string, newConnectionStatus: ConnectionStatus, connectionApi: (userId: string) => IConnection[]) {
    const connections = await connectionApi(userId);

    const user = state.results.items.find(u => u.id == userId);
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