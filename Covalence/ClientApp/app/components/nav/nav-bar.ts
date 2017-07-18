import {computedFrom} from 'aurelia-binding';
import { bindable, autoinject, BindingEngine, PLATFORM, Aurelia } from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';
import { UserService } from '../../services/userService';
import { IUser } from '../../infrastructure/user';


@autoinject
export class NavBar {
    @bindable router = null;
    //subscription: any = {};
    private profile: IUser = null;

    constructor(private auth: AuthService, private bindingEngine: BindingEngine, private userService: UserService, private app: Aurelia) {
        if(this.isAuthenticated) {
            this.profile = this.userService.currentUser;
        }
        //this._isAuthenticated = this.auth.isAuthenticated();
        // this.subscription = this.bindingEngine.propertyObserver(this, 'isAuthenticated')
        //     .subscribe((newValue, oldValue) => {
        //         if (this.isAuthenticated) {
        //             this.auth.getMe().then(data => {
        //                 this.displayName = data.firstName + " " + data.lastName;
        //             });
        //         }
        //     });
    }

    bind() {
        
    }

    attached() {

    }

    @computedFrom('this.profile.firstName', 'this.profile.lastName')
    get displayName() {
        if(this.isAuthenticated && this.profile)
            return `${this.profile.firstName} ${this.profile.lastName}`;
        else
            return "";
    }

    //@computedFrom('authService.authenticated')
    get isAuthenticated() {
        return this.auth.isAuthenticated();
    }

    logout() {
        this.userService.currentUser = null;
        this.auth.logout();

        this.router.navigate('/', { replace: true, trigger: false });
        this.router.reset();
        //this.router.deactivate();

        this.app.setRoot(PLATFORM.moduleName('app/app'));
    }

    deactivate() {
        //this.subscription.dispose();
    }

}