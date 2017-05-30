import {AuthService} from 'aurelia-authentication';
import {inject, computedFrom} from 'aurelia-framework';
import {User, IUser, IUserData} from '../../infrastructure/user';

@inject(AuthService)
export class Login {
    email: string = "";
    password: string = "";

    providers: any[] = [];

    constructor(private authService: AuthService) {

    }

    @computedFrom('authService.authenticated')
    get authenticated() {
        return this.authService.authenticated;
    }

    login() {
        let credentials = { username: this.email, password: this.password, grant_type: "password", scope: "offline_access", resource: "http://localhost:5000" };
        return this.authService.login(credentials)
            .then(me => {
                console.log(me);
            });
    }

    authenticate(name) {
        return this.authService.authenticate(name)
            .then(response => {
                this.providers[name] = true;            
            });
    }
}