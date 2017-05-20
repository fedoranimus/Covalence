import {autoinject, computedFrom} from 'aurelia-framework';
//import {MoleculeBackground} from './molecule-background';
import {TagService} from '../../services/tagService';
import {Router} from 'aurelia-router';
import {AuthService} from 'aurelia-authentication';

@autoinject
export class Home {
    suggestedTags = [];
    tags = [];

    zipCode = "";

    moleculeBackground: HTMLElement;

    constructor(private tagService: TagService, private router: Router, private authService: AuthService) {

    }

    async bind() {
        if(this.authenticated) { //check if user is logged in
            try {
                let user = await this.authService.getMe();
                this.tags = user.studyTags;
                console.debug("Authenticated", user);
            } catch(e) {
                //console.error(e);
            }
        }

        try {
            let tags = await this.tagService.getAllTags();
            this.suggestedTags = tags;
            console.log(tags);
        } catch(e) {
            //console.error(e);
        }  
    }

    async attached() {
        //new MoleculeBackground(this.moleculeBackground);
    }

    @computedFrom('authService.authenticated')
    get authenticated() {
        return this.authService.authenticated;
    }

    search() {
        let searchParams = { tags: this.tags, zip: this.zipCode };
        if(this.authenticated) {
            //get mentor tags and location -> zip code
        } else {
            localStorage.setItem('anonSearch', JSON.stringify(searchParams));
            this.router.navigateToRoute('login');
        }
        console.log(searchParams);
    }

    onRemoveTag(event: CustomEvent) { //this isn't needed, but should be documented
        let tag = event.detail.value;
    }

    onAddTag(event: CustomEvent) {
        let tag = event.detail.value;
        if(this.suggestedTags.indexOf(tag) == -1) {
            //this.tagService.createTag(tag);
            this.suggestedTags.push(tag);
        }
    }
}