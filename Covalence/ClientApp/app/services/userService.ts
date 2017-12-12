import {autoinject} from 'aurelia-framework';
import {Config} from 'aurelia-api';
import {ITag} from '../infrastructure/tag';
import {IUser, IUserViewModel} from '../infrastructure/user';
import { AuthService } from 'aurelia-authentication';

@autoinject
export class UserService {
    private _userStore: IUser|null = null;

    constructor(private config: Config, private authService: AuthService) {
        if(authService.isAuthenticated()) {
            let user = JSON.parse(localStorage.getItem('user'));
            if(!user) {
                authService.getMe().then(currentUser => {
                    localStorage.setItem('user', JSON.stringify(currentUser));
                    this._userStore = currentUser;
                }); 
            } else {
                this._userStore = user;
            }
        }     
    }

    get currentUser() {
        if(this._userStore == null)
            this._userStore = JSON.parse(localStorage.getItem('user'));
            
        return this._userStore;
    }

    set currentUser(user: IUser) {
        if(!user)
            localStorage.removeItem('user')
        else 
            localStorage.setItem('user', JSON.stringify(user));
        
        this._userStore = user;
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
        this.currentUser = await user;
    }

    public forgotPassword(email: string) {
        this.config.getEndpoint('api').post('account/forgotpassword', { email: email });
    }

    public async onboardUser(viewModel: IUserViewModel): Promise<void> {
        viewModel.needsOnboarding = false;
        this.currentUser = await this.config.getEndpoint('api').updateOne('user', this.currentUser.id, null, viewModel);
    }
}