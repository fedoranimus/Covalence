import { autoinject, bindable, computedFrom, lazy } from 'aurelia-framework';
import { AuthService } from 'aurelia-authentication';
import { ValidationControllerFactory, ValidationController, Validator, validateTrigger, ValidationRules, ValidateResult } from 'aurelia-validation';
import { IUser} from 'infrastructure/user';
import { UserService } from 'services/userService';
import { State } from 'store/state';
import { Store } from 'aurelia-store';
import { TagService } from 'services/tagService';
import { Router } from 'aurelia-router';
import { updateUser } from 'store/userActions';


@autoinject
export class Profile {
    model: ProfileModel = {
        firstName: "",
        lastName: "",
        location: "",
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
        this.store.state.subscribe(state => {
            if(state.user) {
                this.model.firstName = state.user.firstName;
                this.model.lastName = state.user.lastName;
                this.model.location = state.user.location;
                this.model.bio = state.user.bio;
                this.model.tags = state.user.tags.map(t => t.name);
            }
        });
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
            .ensure('location').required().maxLength(5).minLength(5)
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

    public async save() {
        this.isLoading = true;
        const model = this.model;
        await this.store.dispatch(updateUser, model, (model) => this.userService.updateUser(model));
        this.isLoading = false;
    }
}

export interface ProfileModel {
    firstName: string;
    lastName: string;
    location: string;
    bio: string;
    tags: string[];
}