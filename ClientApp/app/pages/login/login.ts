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
        return this.authService.login(this.email, this.password)
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