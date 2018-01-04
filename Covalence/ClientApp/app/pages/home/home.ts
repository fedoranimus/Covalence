import {autoinject, computedFrom, bindable} from 'aurelia-framework';
import {TagService} from 'services/tagService';
import {Router} from 'aurelia-router';
import {AuthService} from 'aurelia-authentication';

@autoinject
export class Home {
    suggestedTags = [];
    tags = [];

    @bindable emailAddress: string = "";

    constructor(private tagService: TagService, private router: Router, private authService: AuthService) {

    }

    get isAuthenticated() {
        return this.authService.isAuthenticated();
    }

    @computedFrom('authService.authenticated')
    get authenticated() {
        return this.authService.authenticated;
    }

    get currentYear() {
        return new Date().getFullYear();
    }
}