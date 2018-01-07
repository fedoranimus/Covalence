import { State } from 'store/state';
import { ConnectionStatus, IConnectionList } from 'services/connectionService';

export async function loadConnections(state: State, getConnectionList: Function) {
    const connections = await getConnectionList();

    return { ...state, ...{ connections } };
}

export async function acceptConnection(state: State, requestingUserId: string, acceptConnectionApi: Function) {
    const connections = await acceptConnectionApi(requestingUserId);

    const user = state.results.items.find(u => u.id == requestingUserId);
    user.connectionStatus = ConnectionStatus.connected;

    const index = state.results.items.findIndex(u => u.id == requestingUserId);

    const newState = Object.assign({}, state, { 
        results: {
            ...state.results,
            items: [
                ...state.results.items.slice(0, index),
                Object.assign({}, user),
                ...state.results.items.slice(index + 1)
            ]
        },
        connections: connections
    });

    return newState;
}

export async function requestConnection(state: State, requestConnectionApi: Function) {
    const connections = await requestConnectionApi();

    return Object.assign({}, state, { connections });
}

export async function rejectConnection(state: State, rejectConnectionApi: Function) {
    const connections = await rejectConnectionApi();

    return Object.assign({}, state, { connections });
}

export async function cancelConnectionRequest(state: State, cancelConnectionRequestApi: Function) {
    const connections = await cancelConnectionRequestApi();

    return Object.assign({}, state, { connections });
}

export async function updateConnection(state: State, userId: string, newConnectionStatus: ConnectionStatus, connectionApi: (userId: string) => IConnectionList) {
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