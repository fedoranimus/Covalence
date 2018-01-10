import { IUser, IRemoteUser } from 'infrastructure/user';
import { IConnection } from 'services/connectionService';
import { PagedList } from 'services/searchService';

export const initialState = {
    searchQuery: [],
    user: null,
    connections: [],
    results: null
};

export interface State {
    searchQuery: string[];
    user: IUser;
    connections: IConnection[];
    results: PagedList<IRemoteUser>;
}