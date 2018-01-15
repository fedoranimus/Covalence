import { UserService } from './../../../services/userService';
import { TagService } from './../../../services/tagService';
import { bindable, autoinject } from "aurelia-framework";
import { Validator, ValidationController, ValidationControllerFactory, ValidationRules, validateTrigger } from 'aurelia-validation';
import { Router } from 'aurelia-router';
import { State } from 'store/state';
import { Store } from 'aurelia-store';
import { completeOnboarding } from 'store/userActions';

@autoinject
export class Onboarding {
    model: OnboardModel = {
        firstName: "",
        lastName: "",
        zipCode: "",
        bio: "",
        tags: []
    }

    canSave: boolean = false;
    isLoading: boolean = false;
    private controller: ValidationController;

    constructor(private validator: Validator, 
                private controllerFactory: ValidationControllerFactory, 
                private tagService: TagService, 
                private userService: UserService, 
                private router: Router,
                private store: Store<State>) {
        this.controller = controllerFactory.createForCurrentScope(validator);
        this.controller.validateTrigger = validateTrigger.changeOrBlur;
        this.controller.subscribe(event => this.validate());
    }

    activate() {
        this.setupValidation();
    }

    private validate() {
        this.validator.validateObject(this.model)
            .then(results => this.canSave = results.every(result => result.valid));
    }

    private setupValidation() {
        ValidationRules
            .ensure('firstName').required()
            .ensure('lastName').required()
            .ensure('zipCode').required().maxLength(5).minLength(5)
            .ensure('bio').required()
            .ensure('tags').required().minItems(1)
            .on(this.model);
    }

    async onAddTag(tagName: string) {
        const index = this.model.tags.findIndex(x => x == tagName);
        
        if(index === -1) {
            const tag = await this.tagService.getTag(tagName);
            this.model.tags.push(tag.name);
        }      

        this.validate();
    }

    onRemoveTag(tagName: string) {
        const index = this.model.tags.findIndex(x => x == tagName);
        this.model.tags.splice(index, 1);

        this.validate();
    }

    public async onboard() {
        this.isLoading = true;
        const model = this.model;
        await this.store.dispatch(completeOnboarding, model, (model) => this.userService.onboardUser(model));
        this.isLoading = false;
        this.router.navigate('/');
    }
}

interface OnboardModel {
    firstName: string;
    lastName: string;
    zipCode: string;
    bio: string;
    tags: string[];
}