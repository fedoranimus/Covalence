import {computedFrom} from 'aurelia-binding';
import { bindable, autoinject, BindingEngine, PLATFORM, Aurelia } from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';
import { UserService } from 'services/userService';
import { IUser } from 'infrastructure/user';
import { State } from 'store/state';
import { Store } from 'aurelia-store';


@autoinject
export class NavBar {
    @bindable router = null;
    private state: State;

    constructor(private auth: AuthService, private bindingEngine: BindingEngine, private userService: UserService, private app: Aurelia, private store: Store<State>) {
        store.state.subscribe(state => {
            this.state = state;
        });
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


    @computedFrom('state.user.firstName', 'state.user.lastName')
    get hasDisplayName() {
        if(this.auth.authenticated && this.state.user && this.state.user.firstName && this.state.user.lastName)
            return true;
        
        return false;
    }

    @computedFrom('state.user.emailConfirmed')
    get emailConfirmed() {
        if(this.auth.authenticated && this.state.user && !this.state.user.emailConfirmed)
            return false;

        return true;
    }

    @computedFrom('authService.authenticated')
    get isAuthenticated() {
        return this.auth.authenticated;
    }

    logout() {
        this.auth.logout();

        // TODO - Unregister actions

        this.router.navigate('/', { replace: true, trigger: false });
        this.router.reset();
        //this.router.deactivate();

        this.app.setRoot(PLATFORM.moduleName('app/app'));
    }

    deactivate() {
        
        //this.subscription.dispose();
    }

}