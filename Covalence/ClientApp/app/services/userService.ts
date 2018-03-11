import {autoinject} from 'aurelia-framework';
import {Config} from 'aurelia-api';
import {ITag} from 'infrastructure/tag';
import {IUser, IUserViewModel, IRemoteUser} from '../infrastructure/user';
import { AuthService } from 'aurelia-authentication';
import { State } from 'store/state';
import { Store } from 'aurelia-store';

@autoinject
export class UserService {
    private currentUser: IUser | null;

    constructor(private config: Config, private authService: AuthService, private store: Store<State>) {
        store.state.subscribe(state => {
            this.currentUser = state.user;
        });
    }

    public getUser(userId: string): Promise<IRemoteUser> {
        return this.config.getEndpoint('api').findOne('user', userId);
    }

    public getCurrentUser(): Promise<IUser> {
        return this.config.getEndpoint('api').findOne('user', this.currentUser.id);
    }

    public updateCurrentUser(viewModel: IUserViewModel): Promise<IUser> {
        return this.config.getEndpoint('api').updateOne('user', this.currentUser.id, null, viewModel);
    }

    public forgotPassword(email: string) {
        this.config.getEndpoint('api').post('account/forgotpassword', { email: email });
    }

    public async onboardUser(viewModel: IUserViewModel): Promise<IUser> {
        viewModel.needsOnboarding = false;
        return await this.config.getEndpoint('api').updateOne('user', this.currentUser.id, null, viewModel);
    }
}