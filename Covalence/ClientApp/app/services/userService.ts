import {autoinject} from 'aurelia-framework';
import {Config} from 'aurelia-api';
import {ITag} from 'infrastructure/tag';
import {IUser, IUserViewModel} from '../infrastructure/user';
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

    public getUser(userId: string): Promise<IUser> {
        return this.config.getEndpoint('api').findOne('user', userId);
    }

    public updateUser(viewModel: IUserViewModel): Promise<IUser> {
        return this.config.getEndpoint('api').updateOne('user', this.currentUser.id, null, viewModel);
    }

    public updateUserTags(tagList: string[]): Promise<IUser> {   
        const user = this.config.getEndpoint('api').updateOne('user/tags', this.currentUser.id, null, tagList);
        this.updateCurrentUserTags(user);
        return user;
    }

    private async updateCurrentUserTags(user: Promise<IUser>) {

    }

    public forgotPassword(email: string) {
        this.config.getEndpoint('api').post('account/forgotpassword', { email: email });
    }

    public async onboardUser(viewModel: IUserViewModel): Promise<void> {
        viewModel.needsOnboarding = false;
        this.currentUser = await this.config.getEndpoint('api').updateOne('user', this.currentUser.id, null, viewModel);
    }
}