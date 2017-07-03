import {autoinject, bindable, computedFrom} from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';
import {IUser} from '../../infrastructure/user';
import { UserService } from '../../services/userService';


@autoinject
export class Profile {
    @bindable profile: IUser;
    @bindable isEditable: boolean = false;

    constructor(private authService: AuthService, private userService: UserService) {

    }

    activate() {
        this.profile = this.userService.currentUser;
        // return this.authService.getMe().then(me => {
        //     this.profile = new User(me);
        // });
    }

    @computedFrom('this.profile.firstName', 'this.profile.lastName')
    get displayName() {
        return `${this.profile.firstName} ${this.profile.lastName}`;
    }

    updateDetails() {
        
    }
}