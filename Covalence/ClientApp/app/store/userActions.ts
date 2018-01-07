import { State } from 'store/state';
import { IUser } from 'infrastructure/user';

export async function updateUser(state: State, user: IUser | null) {
    return { ...state, ...{ user } };
}

export async function getCurrentUser(state: State, getMeApi: () => IUser) {
    try {
        const user = await getMeApi();
        return { ...state, ...{ user }};
    } catch (e) {
        console.error(e);
        return state;
    }
}