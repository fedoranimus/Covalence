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
    constructor(private tagService: TagService, private userService: UserService) {

    }

    @computedFrom('tags')
    get hasTags() {
        return this.tags.length > 0;
    }

    async toggleEdit() {
        if(this.canEdit) {
            await this.userService.updateUserTags(this.tags.map(({ name }) => name));
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
        const query = event.detail;
        if(query)
            this.suggestedTags = await this.tagService.queryTag(query);
    }
}