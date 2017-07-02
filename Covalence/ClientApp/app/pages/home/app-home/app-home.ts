import {AuthService} from 'aurelia-authentication';
import {autoinject, computedFrom} from 'aurelia-framework';

@autoinject
export class AppHome {
    public tags = [];
    constructor(private authService: AuthService) {

    }

    async bind() {
        if(this.authenticated) { //check if user is logged in
            try {
                let user = await this.authService.getMe();
                this.tags = user.tags;
                console.debug("Authenticated", user);
            } catch(e) {
                //console.error(e);
            }
        }
    }

    @computedFrom('authService.authenticated')
    get authenticated() {
        return this.authService.authenticated;
    }
}