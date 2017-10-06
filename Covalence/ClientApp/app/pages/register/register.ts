import {AuthService} from 'aurelia-authentication';
import {autoinject} from 'aurelia-framework';

@autoinject
export class Register {
    email: string = "";
    password: string = "";

    constructor(private authService: AuthService) {

    }

    async signup() {
        try {
            await this.authService.signup({
                email: this.email,
                password: this.password
            });
        } catch(e) {
            console.log(e);
        }
    }

}