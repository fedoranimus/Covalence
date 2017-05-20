import {AuthService} from 'aurelia-authentication';
import {autoinject} from 'aurelia-framework';

@autoinject
export class Register {
    email: string = "";
    password: string = "";
    firstName: string = "";
    lastName: string = "";
    occupation: string = "";
    employer: string = "";


    constructor(private authService: AuthService) {

    }

    signup() {
        return this.authService.signup({
            email: this.email,
            password: this.password,
            firstName: this.firstName,
            lastName: this.lastName,
            occupation: this.occupation,
            employer: this.employer
        });
    }

}