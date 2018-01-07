import { IUser, IRemoteUser } from 'infrastructure/user';
import { IConnectionList } from 'services/connectionService';
import { PagedList } from 'services/searchService';

export const initialState = {
    user: null,
    connections: [],
    results: null
};

export interface State {
    user: IUser;
    connections: IConnectionList;
    results: PagedList<IRemoteUser>;
}