import { AuthService } from 'aurelia-authentication';
import { inject, computedFrom, autoinject, PLATFORM, Aurelia } from 'aurelia-framework';
import { IUser } from '../../infrastructure/user';
import { UserService } from '../../services/userService';
import { Router, RouterConfiguration } from 'aurelia-router';
import { ErrorHandler } from '../../utils/errorHandler';
import { ValidationController, ValidationControllerFactory, Validator, ValidationRules, validateTrigger } from 'aurelia-validation';


@autoinject
export class Login {
    model: LoginModel = {
        email: "",
        password: ""
    }

    providers: any[] = [];

    private error: string = null;

    canLogin: boolean = false;
    
    private controller: ValidationController;

    constructor(private authService: AuthService, 
                private userService: UserService, 
                private app: Aurelia, 
                private router: Router, 
                private validator: Validator, 
                private controllerFactory: ValidationControllerFactory) 
    {
        this.controller = controllerFactory.createForCurrentScope(validator);
        this.controller.validateTrigger = validateTrigger.changeOrBlur;
        this.controller.subscribe(event => this.validate());
    }

    @computedFrom('authService.authenticated')
    get authenticated() {
        return this.authService.authenticated;
    }

    activate() {
        this.setupValidation();
    }

    private setupValidation() {
        ValidationRules
            .ensure('email').required().email()
            .ensure('password').required().minLength(6)
            .on(this.model);
    }

    private validate() {
        this.validator.validateObject(this.model)
            .then(results => this.canLogin = results.every(result => result.valid));
    }

    async login() {
        this.error = null;
        const credentials = { username: this.model.email, password: this.model.password, grant_type: "password", scope: "offline_access", resource: "https://localhost:5000" };
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

interface LoginModel {
    email: string;
    password: string;
}