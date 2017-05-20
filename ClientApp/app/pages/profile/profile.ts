import {autoinject, bindable, computedFrom} from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';
import {User, IUser, IUserData} from '../../infrastructure/user';


@autoinject
export class Profile {
    @bindable profile: IUser = new User();
    @bindable isEditable: boolean = false;

    constructor(private authService: AuthService) {

    }

    activate() {
        return this.authService.getMe().then(me => {
            this.profile = new User(me);
        });
    }

    @computedFrom('this.profile.firstName', 'this.profile.lastName')
    get displayName() {
        return `${this.profile.firstName} ${this.profile.lastName}`;
    }

    updateDetails() {
        
    }
}