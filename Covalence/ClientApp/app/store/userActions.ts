import { State } from 'store/state';
import { IUser, IUserViewModel, IRemoteUser } from 'infrastructure/user';

export async function updateCurrentUser(state: State, viewModel: IUserViewModel, updateUserApi: (viewModel: IUserViewModel) => IUser) {
    try {
        const user = await updateUserApi(viewModel);
        return { ...state, ...{ user }};
    } catch(e) {
        return state;
    }
}

export async function getUser(state: State, userId: string, getUserApi: (userId: string) => IRemoteUser) {
    try {
        const remoteUserDetails = await getUserApi(userId);
        return { ...state, ...{ remoteUserDetails }};
    } catch(e) {
        return state;
    }
}

export function clearUser(state: State) {
    return { ...state, ...{ remoteUserDetails: null }};
}

export async function getCurrentUser(state: State, getMeApi: () => IUser) {
    try {
        const user = await getMeApi();
        return { ...state, ...{ user }};
    } catch (e) {
        return state;
    }
}

export function clearCurrentUser(state: State) {
    return { ...state, ...{ user: null }};
}

export async function completeOnboarding(state: State, viewModel: IUserViewModel, onboardUserApi: (viewModel: IUserViewModel) => IUser) {
    try {
        const user = await onboardUserApi(viewModel);
        return { ...state, ...{ user }};
    } catch (e) {
        return state;
    }
}