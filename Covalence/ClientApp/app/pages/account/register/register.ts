import {AuthService} from 'aurelia-authentication';
import {autoinject} from 'aurelia-framework';

@autoinject
export class Register {
    email: string = "";
    password: string = "";

    constructor(private authService: AuthService) {

    }

    signup() {
        return this.authService.signup({
            email: this.email,
            password: this.password
        });
    }

}