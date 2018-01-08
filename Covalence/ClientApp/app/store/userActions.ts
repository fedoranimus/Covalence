import { State } from 'store/state';
import { IUser, IUserViewModel } from 'infrastructure/user';

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

export async function completeOnboarding(state: State, viewModel: IUserViewModel, onboardUserApi: (viewModel: IUserViewModel) => IUser) {
    try {
        const user = await onboardUserApi(viewModel);
        return { ...state, ...{ user }};
    } catch (e) {
        console.error(e);
        return state;
    }
}