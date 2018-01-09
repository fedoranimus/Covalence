import { IUser, IRemoteUser } from 'infrastructure/user';
import { IConnectionList } from 'services/connectionService';
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
    connections: IConnectionList;
    results: PagedList<IRemoteUser>;
}