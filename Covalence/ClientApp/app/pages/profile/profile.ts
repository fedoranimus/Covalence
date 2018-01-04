import { autoinject, bindable, computedFrom } from 'aurelia-framework';
import { AuthService } from 'aurelia-authentication';
import { ValidationControllerFactory, ValidationController, Validator, validateTrigger, ValidationRules, ValidateResult } from 'aurelia-validation';
import { IUser} from 'infrastructure/user';
import { UserService } from 'services/userService';


@autoinject
export class Profile {
    @bindable profile: IUser;
    @bindable isEditable: boolean = false;

    controller: ValidationController = null;

    canSave: boolean = false;

    constructor(private auth: AuthService, private userService: UserService, private validator: Validator, controllerFactory: ValidationControllerFactory) {
        this.controller = controllerFactory.createForCurrentScope();
        this.controller.validateTrigger = validateTrigger.changeOrBlur;
        this.controller.subscribe(event => this.validateForm());
    }

    activate() {
        this.profile = this.userService.currentUser;

        this.setupValidationRules();
    }

    @computedFrom('this.profile.firstName', 'this.profile.lastName')
    get displayName() {
        return `${this.profile.firstName} ${this.profile.lastName}`;
    }

    private setupValidationRules() {
        ValidationRules
            .ensure('firstName').required().minLength(1).withMessage("Please enter a first name")
            .ensure('lastName').required().minLength(1).withMessage("Please enter a last name")
            .ensure('email').required().email().withMessage("Please enter a valid email address")
            .on(this.profile);
    }

    private validateForm() {
        this.validator.validateObject(this.profile)
            .then(results => this.canSave = results.every(result => result.valid));
    }

    set displayName(displayName: string) {
        this.separateNames(displayName);
    }

    async updateDetails() {
        if(!this.canSave) {
            return;
        }

        await this.auth.updateMe(this.profile);
    }

    private separateNames(displayName: string) {
        const names = displayName.split(" ");
        if(names.length > 2) {
            //set error
        } else {
            
        }
    }
}