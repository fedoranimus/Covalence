import { TagService } from 'services/tagService';
import { UserService } from 'services/userService';
import { GoogleMapsService } from 'services/googleMapsService';
import { bindable, autoinject, inject } from "aurelia-framework";
import { Validator, ValidationController, ValidationControllerFactory, ValidationRules, validateTrigger } from 'aurelia-validation';
import { Router } from 'aurelia-router';
import { State } from 'store/state';
import { Store } from 'aurelia-store';
import { completeOnboarding } from 'store/userActions';
import { Location } from 'infrastructure/user';

@autoinject
export class Onboarding {
    model: OnboardModel = {
        firstName: "",
        lastName: "",
        bio: "",
        tags: [],
        latitude: null,
        longitude: null
    }

    locationMarker = [];
    zoomLevel = 15;

    autoUpdateBounds = true;
    shareLocation = true;

    hasLocation: boolean = false;
    canSave: boolean = false;
    isLoading: boolean = false;
    private controller: ValidationController;

    constructor(private validator: Validator, 
                private controllerFactory: ValidationControllerFactory, 
                private tagService: TagService, 
                private userService: UserService, 
                private router: Router,
                private store: Store<State>,
                private mapsApi: GoogleMapsService) {
        this.controller = controllerFactory.createForCurrentScope(validator);
        this.controller.validateTrigger = validateTrigger.changeOrBlur;
        this.controller.subscribe(event => this.validate());
    }

    activate() {
        this.setupValidation();
    }

    attached() {
        this.getGeoLocation();
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

    mapLoaded(map, event) {
        console.log(map, event);
    }

    clickMap(latLng) {
        const lat = latLng.lat();
        const long = latLng.lng();

        this.locationMarker = [{ latitude: lat, longitude: long }];
        this.model.latitude = lat;
        this.model.longitude = long;
    }

    getGeoLocation() {
        this.autoUpdateBounds = true;
        if(navigator.geolocation) {
            navigator.geolocation.getCurrentPosition((position) => this.updatePosition(position));
        }

        this.autoUpdateBounds = false;
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
    bio: string;
    tags: string[];
    latitude: number;
    longitude: number;
}