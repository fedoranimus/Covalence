import {autoinject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';
import {activationStrategy} from 'aurelia-router';
import {TagService} from '../../services/tagService';
import {SearchService} from '../../services/searchService';
import {AuthService} from 'aurelia-authentication';
import {User, IUser, IUserData, ITag} from '../../infrastructure/user';
import {UserService} from '../../services/userService';

@autoinject
export class Search {
    resultType: "mentors"|"proteges" = "mentors";
    suggestedTags = [];
    tags = [];

    zipCode = "";

    matches = [{firstName: "Tim", lastName: "Turner"}, { firstName: "Tim2", lastName: "Turner"}, { firstName: "Tim3", lastName: "Turner"}];

    markers = [];

    private user: IUser = null;

    constructor(private eventAggregator: EventAggregator, private tagService: TagService, private authService: AuthService, private userService: UserService, private searchService: SearchService) {
        tagService.getAllTags().then(tags => {
            this.suggestedTags = tags;
        })
        .catch(console.error);

        
    }
    
    determineActivationStrategy(): string {
        return activationStrategy.invokeLifecycle;
    }

    activate(params, routeConfig, navigationInstruction) {
        this.resultType = routeConfig.name;
        this.authService.getMe().then(me => {
            console.log(me);
            this.user = new User(me);
            let tagObjects = this.resultType == "mentors" ? this.user.studyTags : this.user.expertTags;
            
            //this.zipCode = this.user.zipCode; //convert location to zip code

            this.tags = Object.keys(tagObjects).map((value, index) => {
                return tagObjects[index].name;
            });
        })
        .catch(console.error);

        this.searchService.getResults(this.resultType).then(matches => {
            matches = this.matches;
        })
        .catch(console.error);
    }

    attached() {
        
    }

    findMe() {
        var geoSuccess = (position) => {
            this.user.location = position;
            console.log(position);

            this.markers.push({ latitude: this.user.location.coords.latitude, longitude: this.user.location.coords.longitude, title: this.user.firstName + " " + this.user.lastName });
        };

        navigator.geolocation.getCurrentPosition(geoSuccess);

        
    }

    onAddTag(event: CustomEvent) {
        let tag = event.detail.value;
        let tagType: 'study'|'expert' = "study";
        if(this.resultType == "mentors") {
            tagType = "study";
        } else {
            tagType = "expert"
        }
        console.log(`adding ${tag} for ${tagType}`);
        this.userService.addTag(tag, tagType).then(tags => {
            console.log(tags);
            tagType == "study" ? this.user.studyTags = tags : this.user.expertTags = tags;
        });
    }

    onRemoveTag(event: CustomEvent) {
        let tag = event.detail.value;
        let tagType: 'study'|'expert' = "study";

        if(this.resultType == "mentors") {
            tagType = "study";
        } else {
            tagType = "expert"
        }

        console.log(`removing ${tag} for ${tagType}`);
        this.userService.removeTag(tag, tagType).then(tags => {
            console.log(tags);
            //tagType == "study" ? this.user.studyTags = tags : this.user.expertTags = tags;
        });
    }

    private convertTagNamesToTags(tagNames: string[]): ITag[] {
        return tagNames.map( tagName => {
            return this.convertTagNameToTag(tagName);
        });
    }

    private convertTagNameToTag(tagName: string): ITag {
        return { name: tagName, description: "", _id: "" };
    }
}