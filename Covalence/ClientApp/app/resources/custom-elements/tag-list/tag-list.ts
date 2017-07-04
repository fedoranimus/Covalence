import {computedFrom} from 'aurelia-binding';
import { autoinject, bindable } from 'aurelia-framework';
import { TagService } from '../../../services/tagService';
import { UserService } from '../../../services/userService';
import { ITag } from '../../../infrastructure/tag';

@autoinject
export class TagList {
    public canEdit = false;
    public suggestedTags: ITag[] = [];
    @bindable tags: ITag[] = [];
    public errorState: string|null = null;
    constructor(private tagService: TagService, private userService: UserService) {

    }

    @computedFrom('tags.length')
    get hasTags() {
        return this.tags.length > 0;
    }

    async toggleEdit() {
        if(this.canEdit) {
            await this.userService.updateUserTags(this.tags.map(({ name }) => name.toLowerCase()));
        }

        this.canEdit = !this.canEdit;
    }

    onRemoveTag(tag) {
        const index = this.tags.findIndex(x => x.name == tag.name);
        this.tags.splice(index, 1);
    }

    async onAddTag(event: CustomEvent) {
        const tagName = event.detail;
        const index = this.tags.findIndex(x => x.name == tagName);
        
        if(index === -1) {
            const tag = await this.tagService.getTag(tagName);
            this.tags.push(tag);
        }
            
    }

    async onChangeQuery(event: CustomEvent) {
        const query: string = event.detail;
        if(query && !query.includes(" ")) {
            this.errorState = null;
            const potentialTags = await this.tagService.queryTag(query);
            if(potentialTags)
                this.suggestedTags = potentialTags.filter( t => this.tags.findIndex( x => x.name == t.name) === -1);
        } else {
            this.errorState = "Invalid Query; Spaces are not allowed";
        }
    }
}