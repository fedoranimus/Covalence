import {bindable, autoinject, BindingEngine} from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';

@autoinject
export class NavBar {
    _isAuthenticated = false;
    @bindable router = null;
    subscription: any = {};
    displayName = "Profile";

    constructor(private auth: AuthService, private bindingEngine: BindingEngine) {
        this.auth = auth;
        this._isAuthenticated = this.auth.isAuthenticated();
        this.subscription = this.bindingEngine.propertyObserver(this, 'isAuthenticated')
            .subscribe((newValue, oldValue) => {
                if (this.isAuthenticated) {
                this.auth.getMe().then(data => {
                    this.displayName = data.firstName + " " + data.lastName;
                });
            }
        });
    }

    attached() {
        if(this.isAuthenticated) {
            this.auth.getMe().then(data => {
                this.displayName = data.firstName + " " + data.lastName;
            });
        }
    }

    get isAuthenticated() {
        return this.auth.isAuthenticated();
    }

    logout() {
        return this.auth.logout();
    }

    deactivate() {
        this.subscription.dispose();
    }

}