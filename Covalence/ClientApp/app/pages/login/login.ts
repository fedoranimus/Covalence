import { AuthService } from 'aurelia-authentication';
import { inject, computedFrom, autoinject, PLATFORM, Aurelia } from 'aurelia-framework';
import { IUser } from '../../infrastructure/user';
import { UserService } from '../../services/userService';
import { Router, RouterConfiguration } from 'aurelia-router';
import { ErrorHandler } from '../../utils/errorHandler';


@autoinject
export class Login {
    email: string = "";
    password: string = "";

    providers: any[] = [];

    private error: string = null;

    constructor(private authService: AuthService, private userService: UserService, private app: Aurelia, private router: Router) {

    }

    @computedFrom('authService.authenticated')
    get authenticated() {
        return this.authService.authenticated;
    }

    async login() {
        this.error = null;
        const credentials = { username: this.email, password: this.password, grant_type: "password", scope: "offline_access", resource: "https://localhost:5000" };
        try {
            const token = await this.authService.login(credentials);
            if(token) {
                const user = await this.authService.getMe();
                this.userService.currentUser = user;
    
                this.router.navigate('/', { replace: true, trigger: false });
                this.router.reset();
                //this.router.deactivate();
    
                this.app.setRoot(PLATFORM.moduleName('app/auth-app'));
            }
        } catch(e) {
            this.error = ErrorHandler.formatError(await e.text());
        }
        
    }

    authenticate(name) {
        return this.authService.authenticate(name)
            .then(response => {
                this.providers[name] = true;            
            });
    }
}