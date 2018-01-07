import { State } from "store/state";
import { PagedList } from "services/searchService";
import { IRemoteUser } from "infrastructure/user";

export async function search(state: State, query: string[], searchUsersApi: Function) {
    const results = await searchUsersApi(query);

    return { ...state, ...{ results } };
}

export async function getAll(state: State, getAllUsersApi: Function) {
    const results = await getAllUsersApi();

    return { ...state, ...{ results } };
}

export async function navigateToPage(state: State, pageNumber: number, getAllUsersApi: (pageNumber?: number) => PagedList<IRemoteUser>) {
    const results = await getAllUsersApi(pageNumber);

    return { ...state, ...{ results } };
}