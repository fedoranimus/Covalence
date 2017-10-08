import { UserService } from '../../../services/userService';
import { autoinject } from 'aurelia-framework';

@autoinject
export class ForgotPassword {
    private password: string = "";

    constructor(private userService: UserService) {

    }

    async forgotPassword() {
        await this.userService.forgotPassword(this.password);
    }
}