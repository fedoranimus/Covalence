import {AuthService} from 'aurelia-authentication';
import {autoinject} from 'aurelia-framework';
import { Validator, ValidationController, ValidationControllerFactory, ValidationRules, validateTrigger } from 'aurelia-validation';

@autoinject
export class Register {
    model: RegistrationModel = {
        email: "",
        password: "",
        verifyPassword: ""
    };

    canSave: boolean = false;

    private controller: ValidationController;

    constructor(private authService: AuthService, private validator: Validator, private controllerFactory: ValidationControllerFactory) {
        this.controller = controllerFactory.createForCurrentScope(validator);
        this.controller.validateTrigger = validateTrigger.changeOrBlur;
        this.controller.subscribe(event => this.validate());
    }

    activate(params) {
        this.model.email = params.email;
        this.setupValidation();
    }

    private validate() {
        this.validator.validateObject(this.model)
            .then(results => this.canSave = results.every(result => result.valid));
    }

    private setupValidation() {
        ValidationRules
            .ensure('email').required().email()
            .ensure('password').required().minLength(6).satisfiesRule('specialCharacter').satisfiesRule('number')
            .ensure('verifyPassword').required().satisfiesRule('matchesProperty', 'password')
            .on(this.model);
    }

    async signup() {
        try {
            await this.authService.signup({
                email: this.model.email,
                password: this.model.password
            });
        } catch(e) {
            console.log(e);
        }
    }
}

ValidationRules.customRule(
    'matchesProperty',
    (value, obj, otherPropertyName) => 
      value === null
      || value === undefined
      || value === ''
      || obj[otherPropertyName] === null
      || obj[otherPropertyName] === undefined
      || obj[otherPropertyName] === ''
      || value === obj[otherPropertyName],
    '${$displayName} must match ${$getDisplayName($config.otherPropertyName)}',
    otherPropertyName => ({ otherPropertyName })
  );

  ValidationRules.customRule(
    'specialCharacter',
    (value, obj) => 
        value === null 
        || value === undefined
        || value === ''
        || RegExp('[^a-zA-Z0-9]').test(value),
    '${$displayName} must include at least 1 non-alphanumeric character'
  );

  ValidationRules.customRule(
    'number',
    (value, obj) => 
        value === null
        || value === undefined
        || value === ''
        || RegExp('[0-9]+').test(value),
    '${$displayName} must include at least 1 number'
  );

interface RegistrationModel {
    email: string;
    password: string;
    verifyPassword: string;
}