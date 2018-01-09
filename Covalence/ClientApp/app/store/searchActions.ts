import { State } from "store/state";
import { PagedList } from "services/searchService";
import { IRemoteUser } from "infrastructure/user";

export function addSearchParam(state: State, searchQuery: string) {
    if(state.searchQuery.find(t => t == searchQuery)) {
        return { ...state };
    }

    const newSearchQuery = [ ...state.searchQuery, searchQuery ];

    return { ...state, ...{ searchQuery: newSearchQuery } };
}

export function removeSearchParam(state: State, searchQuery: string) {
    const newSearchQuery = state.searchQuery.filter(param => param != searchQuery);

    return { ...state, ...{ searchQuery: newSearchQuery }};
}

export function clearSearchParams(state: State) {
    return { ...state, ...{ searchQuery: [] }};
}

export async function search(state: State, query: string[], pageNumber: number, searchUsersApi: (query: string[], pageNumber: number) => PagedList<IRemoteUser>) {
    const results = await searchUsersApi(query, pageNumber);

    return { ...state, ...{ results } };
}

export async function navigateToPage(state: State, pageNumber: number, getAllUsersApi: (pageNumber?: number) => PagedList<IRemoteUser>) {
    const results = await getAllUsersApi(pageNumber);

    return { ...state, ...{ results } };
}