import { loadConnections } from 'store/connectionActions';
import { autoinject, bindable, computedFrom, lazy } from 'aurelia-framework';
import { AuthService } from 'aurelia-authentication';
import { ValidationControllerFactory, ValidationController, Validator, validateTrigger, ValidationRules, ValidateResult } from 'aurelia-validation';
import { IUser, Location } from 'infrastructure/user';
import { UserService } from 'services/userService';
import { State } from 'store/state';
import { Store } from 'aurelia-store';
import { TagService } from 'services/tagService';
import { Router } from 'aurelia-router';
import { updateCurrentUser } from 'store/userActions';


@autoinject
export class Settings {
    model: SettingsModel = {
        firstName: "",
        lastName: "",
        bio: "",
        tags: [],
        latitude: 0,
        longitude: 0,
        shareLocation: false
    }

    canSave: boolean = false;
    isLoading: boolean = false;

    locationMarker = [];
    zoomLevel = 8;

    hasLocation: boolean = false;

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
                this.model.bio = state.user.bio;
                this.model.tags = state.user.tags;
                if(!isNaN(state.user.location.latitude) && !isNaN(state.user.location.longitude)) {
                    this.model.latitude = state.user.location.latitude;
                    this.model.longitude = state.user.location.longitude;
                    this.model.shareLocation = true;
                }
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

    mapLoaded(map, event) {
        if(isNaN(this.model.latitude) && isNaN(this.model.longitude))
            this.getGeoLocation();
        else
            this.locationMarker = [{ latitude: this.model.latitude, longitude: this.model.longitude }];
    }

    clickMap(latLng) {
        const lat = latLng.lat();
        const long = latLng.lng();

        this.locationMarker = [{ latitude: lat, longitude: long }];
        this.model.latitude = lat;
        this.model.longitude = long;
    }

    getGeoLocation() {
        if(navigator.geolocation) {
            navigator.geolocation.getCurrentPosition((position) => this.updatePosition(position));
        }
        this.zoomLevel = 8;
    }

    resetLocation() {
        this.model.latitude = null;
        this.model.longitude = null;
        this.hasLocation = false;
    }

    private updatePosition(position: Position) {
        this.hasLocation = true;
        this.model.latitude = position.coords.latitude;
        this.model.longitude = position.coords.longitude;

        this.locationMarker = [{ latitude: position.coords.latitude, longitude: position.coords.longitude }];
    }

    onRemoveTag(tagName: string) {
        const index = this.model.tags.findIndex(x => x == tagName);
        this.model.tags.splice(index, 1);

        this.validate();
    }

    public async save() {
        this.isLoading = true;
        const model = this.model;

        if(!model.shareLocation) {
            model.latitude = null;
            model.longitude = null;
        }

        await this.store.dispatch(updateCurrentUser, model, (model) => this.userService.updateCurrentUser(model));
        this.isLoading = false;
    }
}

export interface SettingsModel {
    firstName: string;
    lastName: string;
    bio: string;
    tags: string[];
    latitude: number;
    longitude: number;
    shareLocation: boolean;
}